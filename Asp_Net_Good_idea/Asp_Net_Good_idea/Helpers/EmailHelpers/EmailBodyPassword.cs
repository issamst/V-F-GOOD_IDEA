namespace Asp_Net_Good_idea.Helpers.Email
{
    public class EmailBodyPassword
    {
        public static string EmailStringBody(string teid, string email, string emailToken)
        {
            return $@"<!doctype html>
<html lang=""en-US"">
<head>
    <meta content=""text/html; charset=utf-8"" http-equiv=""Content-Type"" />
    <title>Reset Password Email Template</title>
    <meta name=""description"" content=""Reset Password Email Template."">
    <style type=""text/css"">
        a:hover {{text-decoration: underline !important;}}
    </style>
</head>
<body marginheight=""0"" topmargin=""0"" marginwidth=""0"" style=""margin: 0px; background-color: #f2f3f8;"" leftmargin=""0"">
    <!--100% body table-->
    <table cellspacing=""0"" border=""0"" cellpadding=""0"" width=""100%"" bgcolor=""#f2f3f8""
        style=""@import url(https://fonts.googleapis.com/css?family=Rubik:300,400,500,700|Open+Sans:300,400,600,700); font-family: 'Open Sans', sans-serif;"">
        <tr>
            <td>
                <table style=""background-color: #f2f3f8; max-width:670px;  margin:0 auto;"" width=""100%"" border=""0""
                    align=""center"" cellpadding=""0"" cellspacing=""0"">
                    <tr>
                        <td style=""height:80px;"">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style=""text-align:center;"">
                          <a href=""https://rakeshmandal.com"" title=""logo"" target=""_blank"">
                            <img width=""180"" src=""https://upload.wikimedia.org/wikipedia/commons/thumb/b/b1/TE_Connectivity_logo.svg/1200px-TE_Connectivity_logo.svg.png"" title=""logo"" alt=""logo"">
                          </a>
                        </td>
                    </tr>
                    <tr>
                        <td style=""height:20px;"">&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <table width=""95%"" border=""0"" align=""center"" cellpadding=""0"" cellspacing=""0""
                                style=""max-width:670px;background:#fff; border-radius:3px; text-align:center;-webkit-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);-moz-box-shadow:0 6px 18px 0 rgba(0,0,0,.06);box-shadow:0 6px 18px 0 rgba(0,0,0,.06);"">
                                <tr>
                                    <td style=""height:40px;"">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td style=""padding:0 35px;"">
                                        <h1 style=""color:#1e1e2d; font-weight:500; margin:0;font-size:32px;font-family:'Rubik',sans-serif;"">You have
                                            requested to reset your password ,TEID :  {teid}</h1>
                                        <span
                                            style=""display:inline-block; vertical-align:middle; margin:29px 0 26px; border-bottom:1px solid #cecece; width:100px;""></span>
                                        <p style=""color:#455056; font-size:15px;line-height:24px; margin:0;"">
                                            We cannot simply send you your old password. A unique link to reset your
                                            password has been generated for you. To reset your password, click the
                                            following link and follow the instructions.
                                        </p>
                                        <a href=""http://localhost:4200/resetPassword?teid={teid}&email={email}&code={emailToken}""
                                            style=""background:#20e277;text-decoration:none !important; font-weight:500; margin-top:35px; color:#fff;text-transform:uppercase; font-size:14px;padding:10px 24px;display:inline-block;border-radius:50px;"">
                                           ResetPassword</a></td>
                                </tr>
                                <tr>
                                    <td style=""height:40px;"">&nbsp;</td>
                                </tr>
                            </table>
                        </td>
                    <tr>
                        <td style=""height:20px;"">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style=""text-align:center;"">
                            <p style=""font-size:14px; color:rgba(255, 165, 0, 1); line-height:18px; margin:0 0 0;""> <strong>SERBOUT issam</strong></p>
                        </td>
                    </tr>
                    
                    
                    <tr>
                        <td style=""text-align:center;"">
                            <p style=""font-size:14px; color:rgba(69, 80, 86, 0.7411764705882353); line-height:18px; margin:0 0 0;""> <strong>IT Business Analyst</strong></p>
                        </td>
                    </tr>
                    <tr>
                        <td style=""text-align:center;"">
                            <p style=""font-size:14px; color:rgba(69, 80, 86, 0.7411764705882353); line-height:18px; margin:0 0 0;""> <strong>Automotive (AUT)<br/>
TE Connectivity Morocco<br/>

Lot 118 & 119 Zone Franche Tanger Automotive City (TAC)<br/>

94031 – Tangier</strong></p>
                        </td>
                    </tr>
                    
                    
                    <tr>
                        <td style=""text-align:center;"">
                            <p style=""font-size:14px; color:rgba(255, 165, 0, 1); line-height:18px; margin:0 0 0;""> <strong>Email : <span style=""color:rgba(69, 80, 86, 0.7411764705882353)"">issam.serbout09@gmail.com</span><br/>
Tel : <span style=""color:rgba(69, 80, 86, 0.7411764705882353);"">07 62 20 64 38</span><br/></strong></p>
                        </td>
                    </tr>
                    <tr>
    <td style=""text-align:center;"">
        <strong id=""textToChange"" style=""font-size:14px; color:blue; line-height:18px; margin:0 0 0;"">
            <a href=""http://www.te.com/"" style=""color:rgba(51, 212, 255);"" onclick=""changeColor()""> Te.com</a>
        </strong>
    </td>
</tr>



                    <tr>
                        <td style=""height:80px;"">&nbsp;</td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <!--/100% body table-->
</body>
</html>

";
        }
    }
}
