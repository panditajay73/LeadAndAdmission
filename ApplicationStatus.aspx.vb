Imports System.Data.SqlClient
Imports System.IO

Partial Class ApplicationStatus
    Inherits System.Web.UI.Page
    Private connStr As String = ConfigurationManager.ConnectionStrings("myconnection").ConnectionString
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Check if user is logged in
        If Not IsPostBack Then
            ' Check if the user is logged in
            If Request.QueryString("UserID") Is Nothing OrElse Request.QueryString("Email") Is Nothing OrElse Request.QueryString("UserName") Is Nothing Then
                ' If not logged in, redirect to the login page
                Response.Redirect("Login.aspx")
            End If
        End If
        If Request.QueryString("UserID") IsNot Nothing Then
            Dim registrationID As Integer = Convert.ToInt32(Request.QueryString("UserID"))
            userIcon.ImageUrl = GetUserPhoto(registrationID)
            ' Query database for application status information
            Using conn As New SqlConnection(connStr)
                conn.Open()
                Dim query As String = "SELECT FeeStatus, ApplicationStatus, RegistrationApproved, AdmissionFeeStatus FROM stuRegistration WHERE RegistrationID = @RegistrationID"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@RegistrationID", registrationID)
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            Dim feeStatus As String = reader("FeeStatus").ToString()
                            Dim applicationStatus As String = reader("ApplicationStatus").ToString()
                            Dim registrationApproved As String = reader("RegistrationApproved").ToString()
                            Dim admissionFeeStatus As String = reader("AdmissionFeeStatus").ToString()

                            ' Call a method to set active steps based on the retrieved status
                            SetActiveSteps(feeStatus, applicationStatus, registrationApproved, admissionFeeStatus)
                        End If
                    End Using
                End Using
            End Using
        End If
        Dim userId As String = Request.QueryString("UserID")
        Dim userName As String = Request.QueryString("UserName")
        Dim email As String = Request.QueryString("Email")

        ' Construct the query string
        Dim queryString As String = "?UserID=" & userId & "&UserName=" & userName & "&Email=" & email

        ' Update links with query parameters
        dashboardLink.HRef = "UserDashboard.aspx" & queryString
        fillApplicationButton.HRef = "ApplicationForm.aspx" & queryString ' Keep as it is since no navigation link is specified
        statusLink.HRef = "ApplicationStatus.aspx" & queryString
        forumLink.HRef = "DiscussionForum.aspx" & queryString
        previewLink.HRef = "ApplicationFormPreview.aspx" & queryString
        profileLink.HRef = "UserProfilePage.aspx" & queryString
        profileLink2.HRef = "UserProfilePage.aspx" & queryString
        changePass.HRef = "ChangePassword.aspx" & queryString
    End Sub
    Protected Function GetUserPhoto(ByVal userID As String) As String
        Dim defaultPhoto As String = "~/images/user.png"
        Dim photoPath As String = defaultPhoto

        Try
            Using conn As New SqlConnection(connStr)
                Dim query As String = "SELECT Docxpath FROM StudentEssentialDoc WHERE StudentID = @StudentID AND EssentialDocID = 1"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@StudentID", userID)
                    conn.Open()

                    Dim result = cmd.ExecuteScalar()
                    If result IsNot Nothing Then
                        Dim docFileName As String = result.ToString()
                        Dim filePath As String = "~/images/" & docFileName

                        If File.Exists(Server.MapPath(filePath)) Then
                            photoPath = filePath
                        End If
                    End If
                End Using
            End Using
        Catch ex As Exception
            ' Log or handle the exception
            photoPath = defaultPhoto
        End Try

        Return photoPath
    End Function
    Private Sub SetActiveSteps(ByVal feeStatus As String, ByVal applicationStatus As String, ByVal registrationApproved As String, ByVal admissionFeeStatus As String)
        ' Set the first step as active
        step1.Attributes("class") = "step step-active"
        Dim iconStep1 As HtmlGenericControl = CType(step1.FindControl("iconStep1"), HtmlGenericControl)
        iconStep1.Attributes("class") = "fa-solid fa-file-signature "

        ' Check FeeStatus and update checkpoint 2
        Dim iconStep2 As HtmlGenericControl = CType(step2.FindControl("iconStep2"), HtmlGenericControl)
        If feeStatus.Contains("Reject") Then
            step2.Attributes("class") = "step step-disabled"
            iconStep2.Attributes("class") = "fa-solid fa-money-check "
        ElseIf feeStatus = "Submitted" Then
            step2.Attributes("class") = "step step-active"
            iconStep2.Attributes("class") = "fa-solid fa-money-check "
        Else
            iconStep2.Attributes("class") = "fa-solid fa-money-check"
        End If

        ' Check ApplicationStatus and update checkpoint 3
        Dim iconStep3 As HtmlGenericControl = CType(step3.FindControl("iconStep3"), HtmlGenericControl)
        If applicationStatus.Contains("Reject") Then
            step3.Attributes("class") = "step step-disabled"
            iconStep3.Attributes("class") = "fa-solid fa-paper-plane "
        ElseIf applicationStatus = "DocVerified" Or applicationStatus = "Approved" Or applicationStatus = "AppSubmitted" Or applicationStatus = "Verified" Then
            step3.Attributes("class") = "step step-active"
            iconStep3.Attributes("class") = "fa-solid fa-paper-plane "
        Else
            iconStep3.Attributes("class") = "fa-solid fa-paper-plane"
        End If

        ' Check RegistrationApproved and update checkpoint 4
        Dim iconStep4 As HtmlGenericControl = CType(step4.FindControl("iconStep4"), HtmlGenericControl)
        If applicationStatus.Contains("Reject") Then
            step4.Attributes("class") = "step step-disabled"
            iconStep4.Attributes("class") = "fa-solid fa-file "
        ElseIf applicationStatus = "DocVerified" Or applicationStatus = "Approved" Or applicationStatus = "Verified" Then
            step4.Attributes("class") = "step step-active"
            iconStep4.Attributes("class") = "fa-solid fa-file "
        Else
            iconStep4.Attributes("class") = "fa-solid fa-file"
        End If
        Dim iconStep5 As HtmlGenericControl = CType(step5.FindControl("iconStep5"), HtmlGenericControl)
        If applicationStatus.Contains("Reject") Then
            step5.Attributes("class") = "step step-disabled"
            iconStep5.Attributes("class") = "fa-solid fa-check"
        ElseIf applicationStatus = "Verified" Or applicationStatus = "Approved" Then
            step5.Attributes("class") = "step step-active"
            iconStep5.Attributes("class") = "fa-solid fa-check "
        Else
            iconStep5.Attributes("class") = "fa-solid fa-check"
        End If
        ' Check AdmissionFeeStatus and update checkpoints 5 and 6
        Dim iconStep6 As HtmlGenericControl = CType(step6.FindControl("iconStep6"), HtmlGenericControl)
        Dim iconStep7 As HtmlGenericControl = CType(step7.FindControl("iconStep7"), HtmlGenericControl)
        If admissionFeeStatus.Contains("Reject") Then
            step6.Attributes("class") = "step step-disabled"
            step7.Attributes("class") = "step step-disabled"
            iconStep6.Attributes("class") = "fa-solid fa-wallet "
            iconStep7.Attributes("class") = "fa-solid fa-thumbs-up "
        ElseIf admissionFeeStatus = "Submitted" Then
            step6.Attributes("class") = "step step-active"
            step7.Attributes("class") = "step step-active"
            iconStep6.Attributes("class") = "fa-solid fa-wallet "
            iconStep7.Attributes("class") = "fa-solid fa-thumbs-up "
        Else
            iconStep6.Attributes("class") = "fa-solid fa-wallet"
            iconStep7.Attributes("class") = "fa-solid fa-thumbs-up"
        End If
    End Sub

End Class
