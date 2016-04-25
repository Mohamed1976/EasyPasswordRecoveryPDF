using System.IO;
using iTextSharp.text.exceptions;
using iTextSharp.text.error_messages;
using iTextSharp.text.io;
using iTextSharp.text;
using iTextSharp.text.pdf;
using EasyPasswordRecoveryPDF.Common;

namespace EasyPasswordRecoveryPDF.Pdf
{
    public sealed class PdfInfoReader : PdfReader
    {
        #region [ Constructors ]

        public PdfInfoReader(string file) : base()
        {
            File = file;
        }

        public PdfInfoReader(string file, byte [] password) : base(file, password)
        {
            File = file;
        }

        #endregion

        #region [ Properties ]

        private string file = string.Empty;
        public string File
        {
            get { return file; }
            private set { file = value; }                
        }

        #endregion

        #region [ Methods ]

        public void GetEncryptionProperties(ref EncryptionRecord encryptionRecord)
        {
            try
            {
                IRandomAccessSource byteSource = new RandomAccessSourceFactory()
                    .SetForceRead(false)
                    .CreateBestSource(File);

                tokens = GetOffsetTokeniser(byteSource);
                ReadPdf();
                ReadDecryptedDocObj(ref encryptionRecord);
            }
            catch (BadPasswordException)
            {
                ReadDecryptedDocObj(ref encryptionRecord);
            }
            catch (IOException e)
            {
                const int rValAes256Iso = 6; 
                IOException ex = new 
                    UnsupportedPdfException(MessageLocalization.GetComposedMessage("unknown.encryption.type.r.eq.1", rValAes256Iso));
                if(ex.Message == e.Message)
                    ReadDecryptedDocObj(ref encryptionRecord);
                else
                    throw e;
            }
            GetCounter().Read(encryptionRecord.fileLength);
        }

        /**
         * Utility method that checks the provided byte source to see if it has junk bytes at the beginning.  If junk bytes
         * are found, construct a tokeniser that ignores the junk.  Otherwise, construct a tokeniser for the byte source as it is
         * @param byteSource the source to check
         * @return a tokeniser that is guaranteed to start at the PDF header
         * @throws IOException if there is a problem reading the byte source
         */
        private static PRTokeniser GetOffsetTokeniser(IRandomAccessSource byteSource)
        {
            PRTokeniser tok = new PRTokeniser(new RandomAccessFileOrArray(byteSource));
            int offset = tok.GetHeaderOffset();
            if (offset != 0)
            {
                IRandomAccessSource offsetSource = new WindowRandomAccessSource(byteSource, offset);
                tok = new PRTokeniser(new RandomAccessFileOrArray(offsetSource));
            }
            return tok;
        }

        /**
         * @throws IOException
         */
        private void ReadDecryptedDocObj(ref EncryptionRecord encryptionRecord)
        {
            string s;
            PdfObject o;

            encryptionRecord.MaxPasswordSize = 32;
            encryptionRecord.PasswordCharset = CharsetEncoding.Ascii;
            encryptionRecord.fileLength = tokens.Length;
            encryptionRecord.pdfVersion = tokens.CheckPdfHeader();
            encryptionRecord.metadataIsEncrypted = true;
            encryptionRecord.documentID = null;
            PdfObject encDic = trailer.Get(PdfName.ENCRYPT);
            if (encDic != null && !encDic.ToString().Equals("null"))
            {
                PdfDictionary enc = (PdfDictionary)GetPdfObject(encDic);
                PdfArray documentIDs = trailer.GetAsArray(PdfName.ID);
                if (documentIDs != null)
                {
                    o = documentIDs.GetPdfObject(0);
                    strings.Remove((PdfString)o);
                    s = o.ToString();
                    encryptionRecord.documentID = DocWriter.GetISOBytes(s);
                    if (documentIDs.Size > 1)
                        strings.Remove((PdfString)documentIDs.GetPdfObject(1));
                }

                // just in case we have a broken producer
                if (encryptionRecord.documentID == null)
                    encryptionRecord.documentID = new byte[0];

                PdfObject filter = GetPdfObjectRelease(enc.Get(PdfName.FILTER));
                if (filter.Equals(PdfName.STANDARD))
                {
                    s = enc.Get(PdfName.U).ToString();
                    strings.Remove((PdfString)enc.Get(PdfName.U));
                    encryptionRecord.uValue = DocWriter.GetISOBytes(s);

                    s = enc.Get(PdfName.O).ToString();
                    strings.Remove((PdfString)enc.Get(PdfName.O));
                    encryptionRecord.oValue = DocWriter.GetISOBytes(s);

                    if (enc.Contains(PdfName.OE))
                        strings.Remove((PdfString)enc.Get(PdfName.OE));
                    if (enc.Contains(PdfName.UE))
                        strings.Remove((PdfString)enc.Get(PdfName.UE));
                    if (enc.Contains(PdfName.PERMS))
                        strings.Remove((PdfString)enc.Get(PdfName.PERMS));

                    o = enc.Get(PdfName.P);
                    if (!o.IsNumber())
                        throw new InvalidPdfException(MessageLocalization.GetComposedMessage("illegal.p.value"));
                    encryptionRecord.pValue = ((PdfNumber)o).LongValue;

                    o = enc.Get(PdfName.R);
                    if (!o.IsNumber())
                        throw new InvalidPdfException(MessageLocalization.GetComposedMessage("illegal.r.value"));
                    encryptionRecord.rValue = ((PdfNumber)o).IntValue;

                    switch (encryptionRecord.rValue)
                    {
                        case 2:
                            encryptionRecord.keyLength = 40;
                            encryptionRecord.encryptionType = PdfEncryptionType.StandardEncryption40Bit;
                            break;
                        case 3:
                            o = enc.Get(PdfName.LENGTH);
                            if (!o.IsNumber())
                                throw new InvalidPdfException(MessageLocalization.GetComposedMessage("illegal.length.value"));
                            int lengthValue = ((PdfNumber)o).IntValue;
                            if (lengthValue > 128 || lengthValue < 40 || lengthValue % 8 != 0)
                                throw new InvalidPdfException(MessageLocalization.GetComposedMessage("illegal.length.value"));
                            encryptionRecord.keyLength = lengthValue;
                            encryptionRecord.encryptionType = PdfEncryptionType.StandardEncryption128Bit;
                            break;
                        case 4:
                            PdfDictionary dic = (PdfDictionary)enc.Get(PdfName.CF);
                            if (dic == null)
                                throw new InvalidPdfException(MessageLocalization.GetComposedMessage("cf.not.found.encryption"));
                            dic = (PdfDictionary)dic.Get(PdfName.STDCF);
                            if (dic == null)
                                throw new InvalidPdfException(MessageLocalization.GetComposedMessage("stdcf.not.found.encryption"));
                            if (PdfName.V2.Equals(dic.Get(PdfName.CFM)))
                                encryptionRecord.encryptionType = PdfEncryptionType.StandardEncryption128Bit;
                            else if (PdfName.AESV2.Equals(dic.Get(PdfName.CFM)))
                                encryptionRecord.encryptionType = PdfEncryptionType.AesEncryption128Bit;
                            else
                                throw new UnsupportedPdfException(MessageLocalization.GetComposedMessage("no.compatible.encryption.found"));
                            PdfObject em = enc.Get(PdfName.ENCRYPTMETADATA);
                            if (em != null && em.ToString().Equals("false"))
                                encryptionRecord.metadataIsEncrypted = false;
                            encryptionRecord.keyLength = 128;
                            break;
                        case 5:
                            PdfObject em5 = enc.Get(PdfName.ENCRYPTMETADATA);
                            if (em5 != null && em5.ToString().Equals("false"))
                                encryptionRecord.metadataIsEncrypted = false;
                            encryptionRecord.keyLength = 256;
                            encryptionRecord.encryptionType = PdfEncryptionType.AesEncryption256Bit;
                            encryptionRecord.PasswordCharset = CharsetEncoding.Unicode;
                            encryptionRecord.MaxPasswordSize = 127;
                            break;
                        case 6:
                            PdfObject _em5 = enc.Get(PdfName.ENCRYPTMETADATA);
                            if (_em5 != null && _em5.ToString().Equals("false"))
                                encryptionRecord.metadataIsEncrypted = false;
                            encryptionRecord.keyLength = 256;
                            encryptionRecord.encryptionType = PdfEncryptionType.AesIsoEncryption256Bit;
                            encryptionRecord.PasswordCharset = CharsetEncoding.Unicode;
                            encryptionRecord.MaxPasswordSize = 127;
                            break;
                        default:
                            throw new UnsupportedPdfException(MessageLocalization.GetComposedMessage("unknown.encryption.type.r.eq.1", rValue));
                    }
                }
                else
                {
                    throw new UnsupportedPdfException(MessageLocalization.GetComposedMessage("no.compatible.encryption.found"));
                }
            }
        }

        #endregion
    }
}
