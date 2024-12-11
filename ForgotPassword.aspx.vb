Imports System.Data.SqlClient
Imports System.Net.Mail

Partial Class ForgotPassword
    Inherits System.Web.UI.Page

    Private connStr As String = ConfigurationManager.ConnectionStrings("myconnection").ConnectionString
    Private Const OTP_SESSION_KEY As String = "GeneratedOtp"
    Private Const EMAIL_SESSION_KEY As String = "OtpEmail"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Display forgot password section initially
            forgotPasswordSection.Style.Add("display", "block")
        End If
        Session("Email") = txtEmail.Text.Trim()
    End Sub

    Protected Sub btnSendOtp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSendOtp.Click
        Dim email As String = txtEmail.Text.Trim()

        ' Check if the email exists in the database
        Using con As New SqlConnection(connStr)
            Dim query As String = "SELECT Email FROM StuRegistration WHERE Email = @Email"
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@Email", email)
                con.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                If reader.HasRows Then
                    ' Generate OTP
                    Dim otp As String = GenerateOTP()
                    Session(OTP_SESSION_KEY) = otp
                    Session(EMAIL_SESSION_KEY) = email

                    ' Send OTP to user's email
                    If SendOTPEmail(email, otp) Then
                        ' Show OTP section and hide forgot password section
                        forgotPasswordSection.Style.Add("display", "none")
                        otpSection.Style.Add("display", "block")
                    Else
                        lblMessage.Text = "Failed to send OTP."
                    End If
                Else
                    lblMessage.Text = "Email not found."
                End If
            End Using
        End Using
    End Sub

    Protected Sub btnVerifyOtp_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVerifyOtp.Click
        Dim enteredOtp As String = txtOtpCode.Text.Trim()

        ' Compare the entered OTP with the generated OTP
        If Session(OTP_SESSION_KEY) IsNot Nothing AndAlso enteredOtp = Session(OTP_SESSION_KEY).ToString() Then
            ' OTP verified, show reset password section and hide OTP section
            otpSection.Style.Add("display", "none")
            resetPasswordSection.Style.Add("display", "block")
        Else
            lblOtpMessage.Text = "Invalid OTP."
        End If
    End Sub

    Protected Sub btnResetPassword_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnResetPassword.Click
        ' Update password in the database
        Dim email As String = Session("Email")
        Using con As New SqlConnection(connStr)
            Dim query As String = "UPDATE StuRegistration SET userpassword = @NewPassword WHERE Email = @Email"
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@NewPassword", txtNewPassword.Text.Trim())
                cmd.Parameters.AddWithValue("@Email", email)
                con.Open()

                If cmd.ExecuteNonQuery() > 0 Then
                    lblPasswordMessage.Text = "Password reset successfully."
                    Session.Remove(OTP_SESSION_KEY)
                    Session.Remove(EMAIL_SESSION_KEY)
                Else
                    lblPasswordMessage.Text = "Password reset failed."
                End If
            End Using
            Response.Redirect("Login.aspx")
        End Using
    End Sub

    Private Function GenerateOTP() As String
        Dim random As New Random()
        Dim otp As String = random.Next(100000, 999999).ToString()
        Return otp
    End Function

    Private Function SendOTPEmail(ByVal email As String, ByVal otp As String) As Boolean
        Try
            Dim smtpClient As New SmtpClient("smtp.gmail.com")
            smtpClient.Credentials = New System.Net.NetworkCredential("pandeyajaysdr@gmail.com", "uzcf bbzr dzaf cbxb")
            smtpClient.Port = 587
            smtpClient.EnableSsl = True

            Dim mailMessage As New MailMessage()
            mailMessage.From = New MailAddress("pandeyajaysdr@gmail.com")
            mailMessage.To.Add(email)
            mailMessage.Subject = "Your OTP Code"
            mailMessage.Body = "Your OTP code is: " & otp

            smtpClient.Send(mailMessage)
            Return True
        Catch ex As Exception
            lblMessage.Text = "An error occurred: " & ex.Message
            lblMessage.Visible = True
            Return False
        End Try
    End Function
End Class
