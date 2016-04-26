# EasyPasswordRecoveryPDF
<B>PDF password recovery tool, The smart, the brute and the list.</B>

This password recovery tool is a Windows 10 desktop app (.Net 4.6) which provides an effective way of recovering PDF passwords. It uses the itextsharp library to retrieve the hashed user and owner password, after which these hashed passwords are compared to the by the user supplied passwords using dedicated algorithms (including the latest PDF encryption algorithm ISO 32000-2). The user can supply passwords in four different ways, by manually clicking the lock icon, using dictionaries, regular expressions and using a brute force approach. The dictionary editor is shown below.

<img alt="screenshot" src="https://cloud.githubusercontent.com/assets/15641092/14799987/5c83744c-0b3f-11e6-96ee-733fa5d8f770.png" />

By clicking the lock icon, you can manually enter a password, after which the password can be validated by clicking the validate button.

<img alt="screenshot" src="https://cloud.githubusercontent.com/assets/15641092/14802465/3efc7894-0b51-11e6-83a3-04ddabe1a522.png" />

An interesting feature of this application is the Smart editor. This editor allows you to specify a regular expression which can then be used to generate passwords. The Smart editor is shown below.

<img alt="screenshot" src="https://cloud.githubusercontent.com/assets/15641092/14800611/1820bd9c-0b43-11e6-8863-36e1ad4abb45.png" />

You can add a new regular expression by clicking the plus button, which opens the regular expression editor shown below.
The regular expression editor allows you to preview the matches by pressing the start button.

<img alt="screenshot" src="https://cloud.githubusercontent.com/assets/15641092/14801345/e4aa506c-0b48-11e6-8dab-298991f580f3.png" />

And last but not least, you can use the Brute force editor. It allows you to generate all possible passwords for a particular charset and password length. Brute force iterations are particularly effective for passwords that only consist of digits. When you press the preview button in the Brute force editor, a summary of the used charset and the expected iteration count is shown.

<img alt="screenshot" src="https://cloud.githubusercontent.com/assets/15641092/14800775/5cb5e63e-0b44-11e6-97fd-688c29171823.png" />

For example when you only choose digits in the Brute force editor and you set the password sweep direction from 1 to 9 digits, the following Brute force iterations will be performed.

<img alt="screenshot" src="https://cloud.githubusercontent.com/assets/15641092/14819574/02fb33b8-0bc3-11e6-998e-cc759451049d.png" />




