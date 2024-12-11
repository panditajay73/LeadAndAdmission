Imports System.Data.SqlClient
Imports System.Data
Imports System.IO

Partial Class ApplicationFormPreview
    Inherits System.Web.UI.Page

    Private connStr As String = ConfigurationManager.ConnectionStrings("myconnection").ConnectionString
    Dim applicationPercent As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Retrieve the email from session
            If Request.QueryString("UserID") Is Nothing OrElse Request.QueryString("Email") Is Nothing OrElse Request.QueryString("UserName") Is Nothing Then
                ' If not logged in, redirect to the login page
                Response.Redirect("Login.aspx")
            End If
            Dim UserID As String = Request.QueryString("UserID").ToString()
            userIcon.ImageUrl = GetUserPhoto(UserID)
            LoadStudentImages(UserID)
            LoadPersonalDetails(Request.QueryString("UserID"))
            LoadEducationData()
            LoadAddressData(Request.QueryString("UserID"))
            LoadDocuments()

            Dim applicationStatus As String = GetApplicationStatus(Request.QueryString("UserID"))
            applicationPercent = GetApplicationPercent(Request.QueryString("UserID"))
            If GetApplicationStatus(Request.QueryString("UserID")) = "AppSubmitted" Or GetApplicationStatus(Request.QueryString("UserID")) = "Verified" Or GetApplicationStatus(Request.QueryString("UserID")) = "Approved" Or GetApplicationStatus(Request.QueryString("UserID")) = "Pending" Then
                ' Hide the Modify and Submit buttons
                Button1.Visible = False
                Button2.Visible = False
                Button3.Visible = False
                Button4.Visible = False
                Button5.Visible = False
                btnSubmit.Visible = False
                PreviewInstructions.Visible = "False"
                ReviewLabel.Text = "Application already submitted, now you can wait and check the status."
            End If

            Dim userName As String = Request.QueryString("UserName")
            Dim email As String = Request.QueryString("Email")

            ' Construct the query string
            Dim queryString As String = "?UserID=" & Request.QueryString("UserID") & "&UserName=" & Request.QueryString("UserName") & "&Email=" & Request.QueryString("Email")

            ' Update links with query parameters
            dashboardLink.HRef = "UserDashboard.aspx" & queryString
            fillApplicationButton.HRef = "ApplicationForm.aspx" & queryString ' Keep as it is since no navigation link is specified
            statusLink.HRef = "ApplicationStatus.aspx" & queryString
            forumLink.HRef = "DiscussionForum.aspx" & queryString
            previewLink.HRef = "ApplicationFormPreview.aspx" & queryString
            profileLink.HRef = "UserProfilePage.aspx" & queryString
            profileLink2.HRef = "UserProfilePage.aspx" & queryString
            changePass.HRef = "ChangePassword.aspx" & queryString
        End If
    End Sub
    'Protected Sub lnkPrint_Click(ByVal sender As Object, ByVal e As EventArgs)
    '    ' Perform any backend logic here, if needed
    '    'footer.Visible = False
    '    'footer2.Visible = False
    '    'topbar.Visible = False
    '    'navbar.Visible = False
    '    'header.Visible = False
    '    'Button1.Visible = False
    '    'Button2.Visible = False
    '    'Button3.Visible = False
    '    'Button4.Visible = False
    '    'Button5.Visible = False
    '    ClientScript.RegisterStartupScript(Me.GetType(), "PrintScript", "printDiv('div_print');", True)
    'End Sub

    Protected Sub ModifyPersonalButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Build the redirect URL
        Dim redirectUrl As String = "ApplicationForm.aspx?UserID=" & Server.UrlEncode(Request.QueryString("UserID")) &
                                    "&UserName=" & Server.UrlEncode(Request.QueryString("UserName")) &
                                    "&Email=" & Server.UrlEncode(Request.QueryString("Email")) &
                                    "&tab=0"

        ' Redirect to the new location
        Response.Redirect(redirectUrl)
    End Sub
    Protected Sub ModifyPaymentButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Build the redirect URL
        Dim redirectUrl As String = "ApplicationForm.aspx?UserID=" & Server.UrlEncode(Request.QueryString("UserID")) &
                                    "&UserName=" & Server.UrlEncode(Request.QueryString("UserName")) &
                                    "&Email=" & Server.UrlEncode(Request.QueryString("Email")) &
                                    "&tab=5"

        ' Redirect to the new location
        Response.Redirect(redirectUrl)
    End Sub
    Protected Sub ModifyEducationalButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Build the redirect URL
        Dim redirectUrl As String = "ApplicationForm.aspx?UserID=" & Server.UrlEncode(Request.QueryString("UserID")) &
                                    "&UserName=" & Server.UrlEncode(Request.QueryString("UserName")) &
                                    "&Email=" & Server.UrlEncode(Request.QueryString("Email")) &
                                    "&tab=1"

        ' Redirect to the new location
        Response.Redirect(redirectUrl)
    End Sub
    Protected Sub ModifyContactButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Build the redirect URL
        Dim redirectUrl As String = "ApplicationForm.aspx?UserID=" & Server.UrlEncode(Request.QueryString("UserID")) &
                                    "&UserName=" & Server.UrlEncode(Request.QueryString("UserName")) &
                                    "&Email=" & Server.UrlEncode(Request.QueryString("Email")) &
                                    "&tab=3"

        ' Redirect to the new location
        Response.Redirect(redirectUrl)
    End Sub
    Protected Sub ModifyDocumentButton_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Build the redirect URL
        Dim redirectUrl As String = "ApplicationForm.aspx?UserID=" & Server.UrlEncode(Request.QueryString("UserID")) &
                                    "&UserName=" & Server.UrlEncode(Request.QueryString("UserName")) &
                                    "&Email=" & Server.UrlEncode(Request.QueryString("Email")) &
                                    "&tab=4"

        ' Redirect to the new location
        Response.Redirect(redirectUrl)
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
    Private Function GetApplicationPercent(ByVal userId As Object) As String
        Dim formPercent As String = String.Empty
        Dim query As String = "SELECT ApplicationFormPercent FROM sturegistration WHERE RegistrationId = @UserID"

        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@UserID", userId)
                conn.Open()
                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    formPercent = result.ToString()
                End If
            End Using
        End Using

        Return formPercent
    End Function
    Private Function GetApplicationStatus(ByVal userId As Object) As String
        Dim status As String = String.Empty
        Dim query As String = "SELECT ApplicationStatus FROM sturegistration WHERE RegistrationId = @UserID"

        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@UserID", userId)
                conn.Open()
                Dim result = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    status = result.ToString()
                End If
            End Using
        End Using

        Return status
    End Function
    'Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs)

    '    'If applicationPercent = "100.00" Then

    '    ' Ask for confirmation through server-side logic
    '    Dim confirmSubmit As String = Request.Form("confirm_value")
    '    If MsgBox("Are you sure you want to submit?", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
    '        ' Update the ApplicationStatus in the database
    '        UpdateApplicationStatus()

    '        ' Redirect to ApplicationForm.aspx after successful update
    '        Response.Redirect("ApplicationForm.aspx")
    '    End If
    'End Sub
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Get the current UserID from Session
        Dim userId As String = Request.QueryString("UserID").ToString()

        ' Check FeeStatus and update label or application status accordingly
        Using conn As New SqlConnection(connStr)
            Dim query As String = "SELECT FeeStatus FROM StuRegistration WHERE RegistrationID = @UserID"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@UserID", userId)
                conn.Open()

                Dim feeStatus As String = Convert.ToString(cmd.ExecuteScalar())

                If feeStatus = "Submitted" Then
                    ' Update ApplicationStatus to 'AppSubmitted'
                    UpdateApplicationStatus(userId)

                    ' Set label to Application Submitted
                    SubmitLabel.Text = "Application Submitted!!"
                    Button1.Visible = False
                    Button2.Visible = False
                    Button3.Visible = False
                    Button4.Visible = False
                    Button5.Visible = False
                    btnSubmit.Visible = False
                    PreviewInstructions.Visible = "False"
                    ReviewLabel.Text = "Application already submitted, now you can wait and check the status."
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('Application Submitted Successfully.');", True)
                Else
                    ' Set label to prompt user to submit fee
                    SubmitLabel.Text = "First Fill all the details and Submit Application Fee."
                    btnSubmit.Visible = False
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('First Fill all the details and Submit Application Fee.');", True)
                End If
            End Using
        End Using
    End Sub

    Protected Sub UpdateApplicationStatus(ByVal userId As String)
        ' Update ApplicationStatus in the database
        Using conn As New SqlConnection(connStr)
            Dim query As String = "UPDATE StuRegistration SET ApplicationStatus = 'AppSubmitted' WHERE RegistrationID = @UserID"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@UserID", userId)
                conn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub



    'Protected Sub UpdateApplicationStatus()
    '    ' Get the current UserID from Session
    '    Dim userId As String = Session("UserID").ToString()

    '    ' Establish the database connection
    '    Using conn As New SqlConnection(connStr)
    '        Dim query As String = "UPDATE StuRegistration SET ApplicationStatus = 'AppSubmitted' WHERE RegistrationID = @UserID"
    '        Using cmd As New SqlCommand(query, conn)
    '            cmd.Parameters.AddWithValue("@UserID", userId)
    '            conn.Open()
    '            cmd.ExecuteNonQuery()
    '        End Using
    '    End Using
    'End Sub
    Protected Sub lnkPrint_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Inject JavaScript to handle print visibility dynamically
        ClientScript.RegisterStartupScript(Me.GetType(), "PrintScript", "printDiv('div_print');", True)
    End Sub


    Private Sub LoadDocuments()
        Using connection As New SqlConnection(connStr)
            connection.Open()
            Dim query As String = "SELECT Studocxid, Docxname, Docxpath FROM StudentEssentialDoc WHERE StudentID = @StudentID"
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@StudentID", Request.QueryString("UserID"))
                Using reader As SqlDataReader = command.ExecuteReader()
                    Dim hasRows As Boolean = False
                    Dim filteredDocs As New List(Of Object)()

                    While reader.Read()
                        Dim docPath As String = reader("Docxpath").ToString().ToLower()
                        If Not (docPath.Contains("photo") Or docPath.Contains("sign")) Then
                            ' Create an anonymous object with the fields you need
                            filteredDocs.Add(New With {
                                .DocumentName = reader("Docxname"),
                                .DocumentPath = reader("Docxpath")
                            })
                            hasRows = True
                        End If
                    End While

                    ' Bind the filtered documents to the Repeater
                    If filteredDocs.Count > 0 Then
                        rptDocuments.Visible = True
                        rptDocuments.DataSource = filteredDocs
                        rptDocuments.DataBind()
                    Else
                        rptDocuments.Visible = False
                    End If
                End Using
            End Using
        End Using
    End Sub
    Protected Sub btnShow_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim button As LinkButton = CType(sender, LinkButton)
        Dim documentPath As String = button.CommandArgument

        ' Redirect to the document path
        Response.Redirect(documentPath, False)
    End Sub
    Protected Sub btnDownload_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btn As LinkButton = CType(sender, LinkButton)
        Dim documentPath As String = btn.CommandArgument.ToString()

        ' Ensure the file path is resolved correctly
        Dim fullPath As String = Server.MapPath(documentPath)

        If File.Exists(fullPath) Then
            Dim fileName As String = Path.GetFileName(fullPath)
            Dim fileExtension As String = Path.GetExtension(fullPath)

            ' Set the content type based on file extension
            Dim contentType As String = "application/octet-stream"
            Select Case fileExtension.ToLower()
                Case ".jpg", ".jpeg"
                    contentType = "image/jpeg"
                Case ".png"
                    contentType = "image/png"
                Case ".pdf"
                    contentType = "application/pdf"
                    ' Add more cases for different file types as needed
            End Select

            ' Clear existing content
            Response.Clear()
            Response.ContentType = contentType
            Response.AddHeader("Content-Disposition", "attachment; filename=" & fileName)
            Response.WriteFile(fullPath)
            Response.End()
        Else
            ' Handle file not found case
            Response.Write("File not found.")
            Response.End()
        End If
    End Sub

    Private Sub LoadAddressData(ByVal Registrationid As String)
        Using conn As New SqlConnection(connStr)
            Dim query As String = _
                "SELECT " & _
                "    sr.Radd AS ResidentialAddress, " & _
                "    cm1.Name AS ResidentialCountry, " & _
                "    sm1.Name AS ResidentialState, " & _
                "    ctm1.Name AS ResidentialCity, " & _
                "    sr.Rpincode AS ResidentialPincode, " & _
                "    sr.Padd AS PermanentAddress, " & _
                "    cm2.Name AS PermanentCountry, " & _
                "    sm2.Name AS PermanentState, " & _
                "    ctm2.Name AS PermanentCity, " & _
                "    sr.Ppincode AS PermanentPincode " & _
                "FROM " & _
                "    stuRegistration sr " & _
                "LEFT JOIN CountryMaster cm1 ON sr.Rcountry = cm1.ID " & _
                "LEFT JOIN StateMaster sm1 ON sr.Rstate = sm1.ID " & _
                "LEFT JOIN CityMaster ctm1 ON sr.Rcity = ctm1.ID " & _
                "LEFT JOIN CountryMaster cm2 ON sr.Pcountry = cm2.ID " & _
                "LEFT JOIN StateMaster sm2 ON sr.Pstate = sm2.ID " & _
                "LEFT JOIN CityMaster ctm2 ON sr.Pcity = ctm2.ID " & _
                "WHERE " & _
                "    sr.Registrationid = @Registrationid"

            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@Registrationid", Registrationid)
            conn.Open()

            Dim dt As New DataTable()
            Dim adapter As New SqlDataAdapter(cmd)
            adapter.Fill(dt)

            ' Bind data to Repeaters if rows are found
            If dt.Rows.Count > 0 Then
                Dim residentialView As DataView = New DataView(dt)
                Dim permanentView As DataView = New DataView(dt)

                ' Set Residential and Permanent DataViews
                rptResidentialAddress.DataSource = residentialView
                rptResidentialAddress.DataBind()

                rptPermanentAddress.DataSource = permanentView
                rptPermanentAddress.DataBind()
            End If
        End Using
    End Sub



    Protected Sub LoadEducationData()
        Dim userID As String = Request.QueryString("UserID")

        ' Fetch data for current UserID
        Using conn As New SqlConnection(connStr)
            conn.Open()
            Dim query As String = "SELECT s.Qualification AS EducationName, s.Roll_No AS RollNo, s.PassingYear AS YearOfPassing, s.MaxMarks AS TotalMarks, s.Marks AS ObtainedMarks, (s.Marks * 100 / s.MaxMarks) AS Percentage, b.Name AS CollegeName, s.Board AS Board FROM StudentEducation s join boarduniversity b on b.ID = s.Board WHERE StudentID = @StudentID"
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@StudentID", userID)

            Dim dt As New DataTable()
            Dim adapter As New SqlDataAdapter(cmd)
            adapter.Fill(dt)

            ' Bind the DataTable to the Repeater
            rptEducationDetails.DataSource = dt
            rptEducationDetails.DataBind()
        End Using
    End Sub

    Private Sub LoadStudentImages(ByVal studentID As String)
        Dim photoFound As Boolean = False
        Dim signatureFound As Boolean = False

        Using conn As New SqlConnection(connStr)
            conn.Open()

            ' Check if the student photo and signature exist in the database
            Dim query As String = "SELECT EssentialDocID, Docxpath FROM StudentEssentialDoc WHERE StudentID = @StudentID"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@StudentID", studentID)
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                While reader.Read()
                    Dim docID As Integer = Convert.ToInt32(reader("EssentialDocID"))
                    Dim docFileName As String = reader("Docxpath").ToString()
                    Dim filePath As String = "~/images/" & docFileName

                    If docID = 1 Then
                        ' Photo
                        If File.Exists(Server.MapPath(filePath)) Then
                            imgPhoto.ImageUrl = filePath
                            photoFound = True
                        Else
                            imgPhoto.ImageUrl = "~/images/user.png"
                        End If
                    ElseIf docID = 2 Then
                        ' Signature
                        If File.Exists(Server.MapPath(filePath)) Then
                            imgSignature.ImageUrl = filePath
                            signatureFound = True
                        Else
                            imgSignature.ImageUrl = "~/images/user.png"
                        End If
                    End If
                End While

                ' If no records exist, set default images
                If Not photoFound Then
                    imgPhoto.ImageUrl = "~/images/user.png"
                End If

                If Not signatureFound Then
                    imgSignature.ImageUrl = "~/images/user.png"
                End If
            End Using
        End Using
    End Sub
    Private Sub LoadPersonalDetails(ByVal Registrationid As String)
        Using conn As New SqlConnection(connStr)
            Dim cmd As New SqlCommand("SELECT * FROM StuRegistration WHERE Registrationid = @Registrationid", conn)
            cmd.Parameters.AddWithValue("@Registrationid", Registrationid)
            conn.Open()
            Dim reader As SqlDataReader = cmd.ExecuteReader()

            Dim isAllFieldsFilled As Boolean = True

            If reader.Read() Then
                ' Populate fields
                RegistrationNo.Text = reader("Registrationid").ToString()
                Name.Text = reader("FirstName").ToString()
                txtEmail.Text = reader("Email").ToString()
                Mobile.Text = reader("MobileNo").ToString()
                ApplyFor.Text = reader("Applycourse").ToString()
                Gender.Text = reader("Gender").ToString()
                DateOfBirth.Text = If(Not IsDBNull(reader("DOB")), Convert.ToDateTime(reader("DOB")).ToString("dd/MM/yyyy"), "")
                FatherName.Text = reader("FatherName").ToString()
                MotherName.Text = reader("MotherName").ToString()
                GuardianMobile.Text = reader("GuardianMobile").ToString()
                Religion.Text = If(Not IsDBNull(reader("Religion")), GetReligionName(reader("Religion")), "")
                Category.Text = If(Not IsDBNull(reader("Castcategories")), reader("Castcategories").ToString(), "")

                'payment details
                lblPaymentDate.Text = If(Not IsDBNull(reader("Dated")), Convert.ToDateTime(reader("Dated")).ToString("dd/MM/yyyy"), "")
                Dim firstName As String = reader("FirstName").ToString().Split(" "c)(0) ' Split by space and take the first part
                lblTransactionID.Text = UCase(firstName) & reader("RegistrationID").ToString()
                lblAmountPaid.Text = "₹" & reader("Registrationfee").ToString()
            Else
                isAllFieldsFilled = False
            End If
        End Using
    End Sub
    Public Function GetReligionName(ByVal ID As String) As String
        Dim religionName As String = String.Empty
        Dim query As String = "SELECT Name FROM Religion WHERE ID = @ReligionID"

        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@ReligionID", ID)
                conn.Open()
                Dim result As Object = cmd.ExecuteScalar()
                If result IsNot Nothing Then
                    religionName = result.ToString()
                End If
            End Using
        End Using

        Return religionName
    End Function

End Class
