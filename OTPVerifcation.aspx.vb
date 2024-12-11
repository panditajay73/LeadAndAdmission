Partial Class OTPVerification
    Inherits System.Web.UI.Page

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmit.Click
        ' Sample logic to verify the OTP. You can modify this with actual logic.
        Dim otpCode As String = txtOtp1.Text & txtOtp2.Text & txtOtp3.Text & txtOtp4.Text & txtOtp5.Text & txtOtp6.Text

        ' For demo purposes, assuming the correct OTP is "536247"
        If otpCode = "536247" Then
            lblMessage.Text = "OTP Verified!"
            lblMessage.ForeColor = System.Drawing.Color.Green
        Else
            lblMessage.Text = "Invalid OTP. Please try again."
        End If
    End Sub
End Class
