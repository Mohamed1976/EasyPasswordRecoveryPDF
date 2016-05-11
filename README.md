# EasyPasswordRecoveryPDF
<B>PDF password recovery tool, The smart, the brute and the list.</B>

This password recovery tool is a Windows 10 desktop application which provides a way of recovering PDF passwords. It uses the itextsharp library to retrieve the hashed user and owner password. After which the retrieved hashed passwords are compared to different passwords. The application user can supply passwords in four different ways, manually by clicking the lock icon, using dictionaries, regular expressions and using a brute force approach. The dictionary editor is shown below.

<img alt="screenshot" src="https://cloud.githubusercontent.com/assets/15641092/14799987/5c83744c-0b3f-11e6-96ee-733fa5d8f770.png" />

By clicking the lock icon, you can manually enter a password, after which the password can be validated by clicking the validate button.

<img alt="screenshot" src="https://cloud.githubusercontent.com/assets/15641092/14802465/3efc7894-0b51-11e6-83a3-04ddabe1a522.png" />

You can view the PDF details by double clicking the PDF entry in the PDF file datagrid.

<img alt="screenshot" src="https://cloud.githubusercontent.com/assets/15641092/14801193/d2ec3e4a-0b47-11e6-91b9-83d2203ab3f4.png" />

An interesting feature of this application is the Smart editor. This editor allows you to specify a regular expression which can then be used to generate passwords. The Smart editor is shown below.

<img alt="screenshot" src="https://cloud.githubusercontent.com/assets/15641092/15196565/39774d08-17cd-11e6-8509-08c9a715148d.png" />

You can add a new regular expression by clicking the plus button, which opens the regular expression editor shown below.
The regular expression editor allows you to preview the matches by pressing the start button.

<img alt="screenshot" src="https://cloud.githubusercontent.com/assets/15641092/14801345/e4aa506c-0b48-11e6-8dab-298991f580f3.png" />

And last but not least, you can use the Brute force editor. It allows you to generate all possible passwords for a particular charset and password length. Brute force iterations are particularly effective for passwords that only consist of digits. When you press the preview button in the Brute force editor, a summary of the used charset and the expected iteration count is shown.

<img alt="screenshot" src="https://cloud.githubusercontent.com/assets/15641092/14800775/5cb5e63e-0b44-11e6-97fd-688c29171823.png" />

For example when you only choose digits in the Brute force editor and you set the password sweep direction from 1 to 9 digits, the following Brute force iterations will be performed.

<img alt="screenshot" src="https://cloud.githubusercontent.com/assets/15641092/14819574/02fb33b8-0bc3-11e6-998e-cc759451049d.png" />

The Settings view allows you to modify dictionary passwords to lowercase, UPPERCASE and Titelcase.

<img alt="screenshot" src="https://cloud.githubusercontent.com/assets/15641092/14800933/ac36ec84-0b45-11e6-9ee6-274615aa5348.png" />

This application uses two external libraries. It uses the itextsharp library to retrieve the hashed user and owner password, which is a little overkill. In the future the itextsharp library will be replaced by a more compact PDF parser, if you have any suggestions, please let me know. The second library it uses, is the Generex library, which is used to generate strings matching the specified regular expressions.

<img alt="screenshot" src="https://cloud.githubusercontent.com/assets/15641092/14801070/db6952c0-0b46-11e6-951c-00abbb7568bf.png" />

The clickonce application can be launched from <a href="https://cdn.rawgit.com/Mohamed1976/EasyPasswordRecoveryPDF/master/EasyPasswordRecoveryPDF/publish/publish.htm" target="_blank">here</a>.

Password dictionaries can be found <a href="https://wiki.skullsecurity.org/Passwords" target="_blank">here</a> and <a href="https://crackstation.net/buy-crackstation-wordlist-password-cracking-dictionary.htm" target="_blank">here</a>.

P.S. More details about this app <a href="http://www.codeproject.com/Articles/MohamedKalmoua#Article" target="_blank">here</a>.
