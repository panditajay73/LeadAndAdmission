Imports System.Web.Services
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO

Partial Class ApplicationForm
    Inherits System.Web.UI.Page

    Dim connStr As String = ConfigurationManager.ConnectionStrings("myconnection").ConnectionString
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ViewState("UserID") = Request.QueryString("UserID")
        ViewState("UserName") = Request.QueryString("UserName")
        ViewState("Email") = Request.QueryString("Email")
        If Not IsPostBack Then

            If ViewState("Email") Is Nothing OrElse ViewState("UserID") Is Nothing OrElse ViewState("UserName") Is Nothing Then
                ' Redirect to login page
                Response.Redirect("login.aspx")
            End If
            If GetApplicationStatus(ViewState("UserID")) = "AppSubmitted" Or GetApplicationStatus(ViewState("UserID")) = "Verified" Or GetApplicationStatus(ViewState("UserID")) = "Approved" Or GetApplicationStatus(ViewState("UserID")) = "Pending" Then
                Response.Redirect("ApplicationFormPreview.aspx?UserID=" & ViewState("UserID") & "&UserName=" & ViewState("UserName") & "&Email=" & ViewState("Email"))
            End If

            ' Check if ApplicationStatus is 'AppSubmitted'
            Dim userId As String = ViewState("UserID").ToString()
            userIcon.ImageUrl = GetUserPhoto(userId)
            ' Prepopulate form fields
            txtEmail.Text = ViewState("Email").ToString()
            LoadPersonalDetails(ViewState("Email").ToString())
            LoadReligion()
            LoadCategory()
            LoadEducationData()
            BindBoardDropdown()
            Dim studentID As String = ViewState("UserID")
            LoadStudentImages(studentID)
            PopulateYearDropDown()
            ' Load additional data

            LoadDocuments()
            UpdateHiddenFeeStatus()
            ' Calculate percentage and update in the database
            Dim percentage As Double = CalculateAndReturnPercentage()
            UpdateApplicationFormPercent(ViewState("UserID").ToString(), percentage)

            'CheckFeeStatus()
            Dim activeTab As Integer = GetActiveTabFromRequest()

            ' If the active tab is the Payment tab (index 5)
            If activeTab = 5 Then
                If Not IsPostBack Then
                    ' Get the fee status and store it in a hidden field for JavaScript
                    Dim feeStatus As String = GetFeeStatusFromDatabase(ViewState("UserID").ToString())
                    ViewState("feeStatus") = feeStatus

                    ' Set the hidden field value
                    If feeStatus = "Submitted" Then
                        HiddenPaymentStatus.Value = "1"
                    Else
                        HiddenPaymentStatus.Value = "0"
                    End If
                End If
            End If

            PopulateCountries()
            PopulateCountries2()
            LoadAddressData(ViewState("UserID").ToString())
        End If
        Dim userName As String = Request.QueryString("UserName")
        Dim email As String = Request.QueryString("Email")

        ' Construct the query string
        Dim queryString As String = "?UserID=" & ViewState("UserID") & "&UserName=" & ViewState("UserName") & "&Email=" & ViewState("Email")

        ' Update links with query parameters
        dashboardLink.HRef = "UserDashboard.aspx" & queryString
        fillApplicationButton.HRef = "ApplicationForm.aspx" & queryString  ' Keep as it is since no navigation link is specified
        statusLink.HRef = "ApplicationStatus.aspx" & queryString
        forumLink.HRef = "DiscussionForum.aspx" & queryString
        previewLink.HRef = "ApplicationFormPreview.aspx" & queryString
        profileLink.HRef = "UserProfilePage.aspx" & queryString
        profileLink2.HRef = "UserProfilePage.aspx" & queryString
        changePass.HRef = "ChangePassword.aspx" & queryString
        ' Store percentage in session
        Session("PercentageOfOnes") = CalculateAndReturnPercentage()
    End Sub

    ' Populate Country Dropdown
    Private Sub PopulateCountries()
        Using conn As New SqlConnection(connStr)
            Dim query As String = "SELECT ID, Name FROM CountryMaster"
            Dim cmd As New SqlCommand(query, conn)

            conn.Open()
            Dim reader As SqlDataReader = cmd.ExecuteReader()
            resCountry.DataSource = reader
            resCountry.DataTextField = "Name"
            resCountry.DataValueField = "ID"
            resCountry.DataBind()
            conn.Close()
        End Using

        ' Optionally, insert a default item at the top of the list
        resCountry.Items.Insert(0, New ListItem("--Select Country--", "0"))
    End Sub

    Private Sub PopulateStates(ByVal countryID As String, ByVal stateDropdown As DropDownList)
        If Not String.IsNullOrEmpty(countryID) AndAlso countryID <> "0" Then
            Using conn As New SqlConnection(connStr)
                Dim query As String = "SELECT ID, Name FROM StateMaster WHERE CountryID = @CountryID"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@CountryID", countryID)

                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                stateDropdown.DataSource = reader
                stateDropdown.DataTextField = "Name"
                stateDropdown.DataValueField = "ID"
                stateDropdown.DataBind()
                conn.Close()
            End Using
        End If
        stateDropdown.Items.Insert(0, New ListItem("--Select State--", "0"))
    End Sub

    Private Sub PopulateCities(ByVal stateID As String, ByVal cityDropdown As DropDownList)
        If Not String.IsNullOrEmpty(stateID) AndAlso stateID <> "0" Then
            Using conn As New SqlConnection(connStr)
                Dim query As String = "SELECT ID, Name FROM CityMaster WHERE StateID = @StateID"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@StateID", stateID)

                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                cityDropdown.DataSource = reader
                cityDropdown.DataTextField = "Name"
                cityDropdown.DataValueField = "ID"
                cityDropdown.DataBind()
                conn.Close()
            End Using
        End If
        cityDropdown.Items.Insert(0, New ListItem("--Select City--", "0"))
    End Sub

    Private Sub PopulateCountries2()
        Using conn As New SqlConnection(connStr)
            Dim query As String = "SELECT ID, Name FROM CountryMaster"
            Dim cmd As New SqlCommand(query, conn)

            conn.Open()
            Dim reader As SqlDataReader = cmd.ExecuteReader()
            permCountry.DataSource = reader
            permCountry.DataTextField = "Name"
            permCountry.DataValueField = "ID"
            permCountry.DataBind()
            conn.Close()
        End Using

        ' Optionally, insert a default item at the top of the list
        permCountry.Items.Insert(0, New ListItem("--Select Country--", "0"))
    End Sub



    ' Populate State Dropdown based on CountryID
    Protected Sub permCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles permCountry.SelectedIndexChanged
        Dim countryID As Integer = Convert.ToInt32(permCountry.SelectedValue)
        If countryID > 0 Then
            Using conn As New SqlConnection(connStr)
                Dim query As String = "SELECT ID, Name FROM StateMaster WHERE CountryID = @CountryID"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@CountryID", countryID)

                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                permState.DataSource = reader
                permState.DataTextField = "Name"
                permState.DataValueField = "ID"
                permState.DataBind()
                conn.Close()
            End Using
        End If
        permState.Items.Insert(0, New ListItem("--Select State--", "0"))
        permCity.Items.Clear()
        permCity.Items.Insert(0, New ListItem("--Select City--", "0"))
    End Sub

    ' Populate City Dropdown based on StateID
    Protected Sub permState_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles permState.SelectedIndexChanged
        Dim stateID As Integer = Convert.ToInt32(permState.SelectedValue)
        If stateID > 0 Then
            Using conn As New SqlConnection(connStr)
                Dim query As String = "SELECT ID, Name FROM CityMaster WHERE StateID = @StateID"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@StateID", stateID)

                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                permCity.DataSource = reader
                permCity.DataTextField = "Name"
                permCity.DataValueField = "ID"
                permCity.DataBind()
                conn.Close()
            End Using
        End If
        permCity.Items.Insert(0, New ListItem("--Select City--", "0"))
    End Sub
    Public Function GetRegistrationIDByEmail() As Integer
        ' Ensure the session has a valid email value
        If ViewState("Email") Is Nothing OrElse String.IsNullOrEmpty(ViewState("Email").ToString()) Then
            Response.Redirect("Login.aspx")
        End If

        ' Initialize the connection and SQL command
        Dim registrationID As Integer = 0
        Dim query As String = "SELECT RegistrationID FROM stuRegistration WHERE Email = @Email"

        ' Create connection object
        Using con As New SqlConnection(connStr)
            ' Create the SQL command
            Using cmd As New SqlCommand(query, con)
                ' Add parameter for email
                cmd.Parameters.AddWithValue("@Email", ViewState("Email").ToString())

                ' Open connection
                con.Open()

                ' Execute the query and get the RegistrationID
                Dim result As Object = cmd.ExecuteScalar()
                If result IsNot Nothing AndAlso Integer.TryParse(result.ToString(), registrationID) Then
                    ' Successfully parsed the RegistrationID
                Else
                    ' Handle the case where no RegistrationID was found
                    registrationID = 0 ' or throw an exception if preferred
                End If
            End Using
        End Using

        ' Return the RegistrationID
        Return registrationID
    End Function

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
    Private Function GetActiveTabFromRequest() As Integer
        If Request.QueryString("tab") IsNot Nothing Then
            Dim tabIndex As Integer
            If Integer.TryParse(Request.QueryString("tab"), tabIndex) Then
                Return tabIndex
            End If
        End If
        Return 0 ' Default to first tab
    End Function

    Protected Function IsApplicationSubmitted(ByVal userId As String) As Boolean
        Dim isSubmitted As Boolean = False

        Using conn As New SqlConnection(connStr)
            Dim query As String = "SELECT ApplicationStatus FROM StuRegistration WHERE RegistrationID = @UserID"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@UserID", userId)
                conn.Open()
                Dim status As String = If(cmd.ExecuteScalar() IsNot Nothing, cmd.ExecuteScalar().ToString(), String.Empty)
                ' Check if the status is 'AppSubmitted'
                If status = "AppSubmitted" Then
                    isSubmitted = True
                End If
            End Using
        End Using
        Return isSubmitted
    End Function

    Protected Sub UpdateHiddenFeeStatus()
        ' Get the current user's RegistrationID from the Session
        Dim registrationID As String = ViewState("UserID").ToString()

        ' Initialize the hidden field value
        HiddenPaymentStatus.Value = "0" ' Default to "0" (not submitted)

        ' Create a SQL connection
        Using conn As New SqlConnection(connStr)
            ' SQL query to retrieve FeeStatus for the current RegistrationID
            Dim query As String = "SELECT FeeStatus FROM stuRegistration WHERE RegistrationID = @RegistrationID"
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@RegistrationID", registrationID)

            ' Open the connection and execute the query
            conn.Open()
            Dim reader As SqlDataReader = cmd.ExecuteReader()

            ' If the query returns a result, check the FeeStatus
            If reader.Read() Then
                Dim feeStatus As String = reader("FeeStatus").ToString()

                ' If FeeStatus is "Submitted", update the hidden field to "1"
                If feeStatus = "Submitted" Then
                    HiddenPaymentStatus.Value = "1"
                End If
            End If
        End Using
    End Sub

    Private Sub UpdateApplicationFormPercent(ByVal userID As String, ByVal percentage As Double)
        Using conn As New SqlConnection(connStr)
            conn.Open()

            ' Prepare the SQL command to update the percentage
            Dim query As String = "UPDATE StuRegistration SET ApplicationFormPercent = @Percentage WHERE Registrationid = @Registrationid"
            Using cmd As New SqlCommand(query, conn)
                ' Add parameters
                cmd.Parameters.AddWithValue("@Percentage", percentage.ToString("0.00")) ' Format to 2 decimal places
                cmd.Parameters.AddWithValue("@Registrationid", userID) ' Use Registrationid instead of StudentID

                ' Execute the command
                cmd.ExecuteNonQuery()
            End Using
            CalculateAndReturnPercentage()
        End Using
    End Sub

    Private Function CalculateAndReturnPercentage() As Double
        ' Retrieve the hidden field values
        Dim hidden1 As Integer = Convert.ToInt32(hfFormStatus.Value)
        Dim hidden2 As Integer = Convert.ToInt32(hiddenEducationStatus.Value)
        Dim hidden3 As Integer = Convert.ToInt32(HiddenPhotoSignStatus.Value)
        Dim hidden4 As Integer = Convert.ToInt32(HiddenContactStatus.Value)
        Dim hidden5 As Integer = Convert.ToInt32(HiddenDocumentsStatus.Value)
        Dim hidden6 As Integer = Convert.ToInt32(HiddenPaymentStatus.Value)

        ' Count the number of `1` values
        Dim countOfOnes As Integer = New Integer() {hidden1, hidden2, hidden3, hidden4, hidden5, hidden6}.Count(Function(x) x = 1)

        ' Calculate the percentage
        Dim totalFields As Integer = 6
        Dim percentageOfOnes As Double = (countOfOnes / totalFields) * 100

        Return percentageOfOnes
    End Function


    ' Get the FeeStatus from the database
    Protected Function GetFeeStatusFromDatabase(ByVal registrationID As String) As String
        Dim feeStatus As String = ""

        Try
            Using conn As New SqlConnection(connStr)
                Dim query As String = "SELECT FeeStatus FROM stuRegistration WHERE RegistrationID = @RegistrationID"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@RegistrationID", registrationID)

                    conn.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            feeStatus = reader("FeeStatus").ToString()
                        End If
                    End Using
                End Using
            End Using

            ' Store the status in a hidden field for frontend usage
            If feeStatus = "Submitted" Then
                HiddenPaymentStatus.Value = "1"
            Else
                HiddenPaymentStatus.Value = "0"
            End If

        Catch ex As Exception
            ' Log or handle the exception
            feeStatus = "Error"
        End Try

        Return feeStatus
    End Function


    ' Update FeeStatus when "Pay Application Fee" is clicked
    Protected Sub btnPayApplicationFee_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim registrationID As String = ViewState("UserID").ToString()

        ' Update FeeStatus in the database to "Submitted"
        Using conn As New SqlConnection(connStr)
            Dim query As String = "UPDATE stuRegistration SET FeeStatus = 'Submitted', registrationfee = @RegistrationFee WHERE RegistrationID = @RegistrationID"
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@RegistrationID", registrationID)
            cmd.Parameters.AddWithValue("@RegistrationFee", 1000D)

            conn.Open()
            cmd.ExecuteNonQuery()
        End Using

        ' Reload page to reflect the updated FeeStatus
        Response.Redirect("ApplicationForm.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                         "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                         "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                         "&tab=" & Server.UrlEncode("5"))
    End Sub


    ' Redirect to Application Form Preview
    Protected Sub btnPreviewApplication_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPreviewApplication.Click
        Response.Redirect("ApplicationFormPreview.aspx?UserID=" & ViewState("UserID") & "&UserName=" & ViewState("UserName") & "&Email=" & ViewState("Email"))
    End Sub



    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If fileUpload.HasFile Then
                ' Format the document name and append the UserID right at the start
                Dim docName As String = documentType.SelectedValue
                docName = docName.Replace(" ", "_") ' Replace spaces with underscores

                ' Construct the file name using UserID and formatted document name
                Dim fileName As String = ViewState("UserID") & "_" & docName & Path.GetExtension(fileUpload.FileName).ToLower()

                ' Check the file extension after constructing the fileName
                Dim fileExtension As String = Path.GetExtension(fileName).ToLower()
                If fileExtension <> ".pdf" Then
                    'ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert('Only PDF files are allowed!'); window.location='ApplicationForm.aspx?tab=4';", True)
                    ClientScript.RegisterStartupScript(Me.GetType(), "alert",
    "showCustomAlert('Only PDF files are allowed!'); " & _
    "window.location='ApplicationForm.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
    "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
    "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
    "&tab=4';", True)

                    Return
                End If

                ' Check the file size (must be less than 200KB)
                If fileUpload.PostedFile.ContentLength > 200 * 1024 Then
                    'ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert('File must be less than 200KB.'); window.location='ApplicationForm.aspx?tab=4';", True)
                    ClientScript.RegisterStartupScript(Me.GetType(), "alert",
     "showCustomAlert('File must be less than 200KB!'); " & _
     "window.location='ApplicationForm.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
     "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
     "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
     "&tab=4';", True)

                    Return
                End If

                ' Ensure the folder exists
                Dim folderPath As String = Server.MapPath("~/Documents/")
                If Not Directory.Exists(folderPath) Then
                    Directory.CreateDirectory(folderPath)
                End If

                ' Save the file with the formatted file name
                Dim filePath As String = Path.Combine(folderPath, fileName)
                fileUpload.SaveAs(filePath)

                ' Insert into the database
                InsertDocumentRecord(fileName, filePath)

                ' Reload documents
                LoadDocuments()

                ' Redirecting after upload is successful
                Response.Redirect("ApplicationForm.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                         "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                         "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                         "&tab=" & Server.UrlEncode("4"))
            End If
        Catch ex As Exception
            'ClientScript.RegisterStartupScript(Me.GetType(), "alert", "alert('Error uploading document: " & ex.Message & "'); window.location='ApplicationForm.aspx?tab=4';", True)
            ClientScript.RegisterStartupScript(Me.GetType(), "alert",
    "showCustomAlert('Error uploading document: " & ex.Message.Replace("'", "\'") & "'); " & _
    "window.location='ApplicationForm.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
    "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
    "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
    "&tab=4';", True)


            'Response.Redirect("ApplicationForm.aspx?tab=4")
        End Try
    End Sub

    Protected Sub btnDownload_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Cast the sender to LinkButton
        Dim lnkBtn As LinkButton = CType(sender, LinkButton)

        ' Get the document path from CommandArgument
        Dim documentPath As String = lnkBtn.CommandArgument.ToString()

        ' Resolve the file path on the server
        Dim fullPath As String = Server.MapPath(documentPath)

        ' Check if the file exists
        If File.Exists(fullPath) Then
            Dim fileName As String = Path.GetFileName(fullPath)
            Dim fileExtension As String = Path.GetExtension(fullPath)

            ' Set the correct content type based on the file extension
            Dim contentType As String = "application/octet-stream"
            Select Case fileExtension.ToLower()
                Case ".jpg", ".jpeg"
                    contentType = "image/jpeg"
                Case ".png"
                    contentType = "image/png"
                Case ".pdf"
                    contentType = "application/pdf"
                    ' Add more cases for different file types if needed
            End Select

            ' Prepare the file for download
            Response.Clear()
            Response.ContentType = contentType
            Response.AddHeader("Content-Disposition", "attachment; filename=" & fileName)
            Response.WriteFile(fullPath)
            Response.End()
        Else
            ' Handle the case where the file does not exist
            Response.Write("File not found.")
            Response.End()
        End If
    End Sub




    Private Sub InsertDocumentRecord(ByVal fileName As String, ByVal filePath As String)

        Using connection As New SqlConnection(connStr)
            connection.Open()
            ' Updated query to insert dropdown value into Docxname column instead of the file name
            Dim query As String = "INSERT INTO StudentEssentialDoc (Dated, StudentID, EssentialDocID, PhotoCopy, IsSub, InquiryId, reqid, Docxname, Docxpath, userid) " &
                                  "VALUES (@Dated, @StudentID, @EssentialDocID, @PhotoCopy, @IsSub, @InquiryId, @reqid, @Docxname, @Docxpath, @userid)"
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@Dated", DateTime.Now)
                command.Parameters.AddWithValue("@StudentID", ViewState("UserID"))
                command.Parameters.AddWithValue("@EssentialDocID", 0)
                command.Parameters.AddWithValue("@PhotoCopy", documentType.SelectedValue) ' Dropdown value as photocopy type
                command.Parameters.AddWithValue("@IsSub", 1)
                command.Parameters.AddWithValue("@InquiryId", DBNull.Value) ' Set as per your requirement
                command.Parameters.AddWithValue("@reqid", DBNull.Value) ' Set as per your requirement
                ' Insert the dropdown value as the Docxname (document type)
                command.Parameters.AddWithValue("@Docxname", documentType.SelectedItem.Text)
                ' Store the actual file path in Docxpath
                command.Parameters.AddWithValue("@Docxpath", "~/Documents/" & fileName)
                command.Parameters.AddWithValue("@userid", ViewState("UserID")) ' Assuming current session UserID is StudentID

                command.ExecuteNonQuery()
            End Using
        End Using
    End Sub


    Private Sub LoadDocuments()

        Using connection As New SqlConnection(connStr)
            connection.Open()
            Dim query As String = "SELECT Studocxid, Docxname, Docxpath FROM StudentEssentialDoc WHERE StudentID = @StudentID"
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@StudentID", ViewState("UserID"))
                Using reader As SqlDataReader = command.ExecuteReader()
                    Dim hasRows As Boolean = False
                    Dim filteredDocs As New List(Of Object)()

                    ' Get uploaded documents to disable in dropdown
                    Dim uploadedDocuments As New HashSet(Of String)()

                    While reader.Read()
                        Dim docPath As String = reader("Docxpath").ToString().ToLower()
                        If Not (docPath.Contains("photo") Or docPath.Contains("sign")) Then
                            ' Create an anonymous object with the fields you need
                            filteredDocs.Add(New With {
                                .Studocxid = reader("Studocxid"),
                                .Docxname = reader("Docxname"),
                                .Docxpath = reader("Docxpath")
                            })
                            uploadedDocuments.Add(reader("Docxname").ToString())
                            hasRows = True
                        End If
                    End While

                    ' Bind the filtered documents to the Repeater
                    If filteredDocs.Count > 0 Then
                        docPanel.Visible = True
                        docRepeater.DataSource = filteredDocs
                        docRepeater.DataBind()
                    Else
                        docPanel.Visible = False
                    End If

                    ' Disable uploaded documents in the dropdown
                    For Each item As ListItem In documentType.Items
                        If uploadedDocuments.Contains(item.Text) Then
                            item.Enabled = False
                        End If
                    Next

                    ' Set HiddenDocumentsStatus based on whether any rows were found
                    HiddenDocumentsStatus.Value = If(hasRows, "1", "0")
                End Using
            End Using
        End Using
    End Sub




    Protected Sub DeleteDocument(ByVal sender As Object, ByVal e As EventArgs)
        Dim Studocxid As Integer = Convert.ToInt32(CType(sender, LinkButton).CommandArgument)

        Using connection As New SqlConnection(connStr)
            connection.Open()
            ' Retrieve the file path from the database
            Dim query As String = "SELECT Docxpath FROM StudentEssentialDoc WHERE Studocxid = @Studocxid"
            Dim filePath As String = ""
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@Studocxid", Studocxid)
                filePath = Convert.ToString(command.ExecuteScalar())
            End Using

            ' Delete the file
            If Not String.IsNullOrEmpty(filePath) Then
                Dim fullPath As String = Server.MapPath(filePath)
                If File.Exists(fullPath) Then
                    File.Delete(fullPath)
                End If
            End If

            ' Delete the record from the database
            query = "DELETE FROM StudentEssentialDoc WHERE Studocxid = @Studocxid"
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@Studocxid", Studocxid)
                command.ExecuteNonQuery()
            End Using
        End Using

        ' Reload documents
        LoadDocuments()
    End Sub

    Private Sub LoadAddressData(ByVal registrationid As String)
        Using conn As New SqlConnection(connStr)
            Dim cmd As New SqlCommand("SELECT * FROM sturegistration WHERE Registrationid = @Registrationid", conn)
            cmd.Parameters.AddWithValue("@Registrationid", registrationid)
            conn.Open()

            Dim reader As SqlDataReader = cmd.ExecuteReader()
            If reader.Read() Then
                ' Load Residential Address
                resAddress.Text = reader("Radd").ToString()
                Dim residentialCountry As String = reader("Rcountry").ToString()
                Dim residentialState As String = reader("RState").ToString()
                Dim residentialCity As String = reader("Rcity").ToString()
                resPinCode.Text = reader("Rpincode").ToString()

                ' Populate Residential Dropdowns if Null or Empty
                If String.IsNullOrWhiteSpace(residentialCountry) Then
                    PopulateCountries()
                Else
                    resCountry.SelectedValue = residentialCountry
                End If
                PopulateStates(residentialCountry, resState)
                If String.IsNullOrWhiteSpace(residentialState) Then
                    resState.Items.Clear()
                    resState.Items.Insert(0, New ListItem("--Select State--", "0"))
                Else
                    resState.SelectedValue = residentialState

                End If
                PopulateCities(residentialState, resCity)
                If String.IsNullOrWhiteSpace(residentialCity) Then
                    resCity.Items.Clear()
                    resCity.Items.Insert(0, New ListItem("--Select City--", "0"))
                Else
                    resCity.SelectedValue = residentialCity
                End If

                ' Load Permanent Address
                permAddress.Text = reader("Padd").ToString()
                Dim permanentCountry As String = reader("Pcountry").ToString()
                Dim permanentState As String = reader("PState").ToString()
                Dim permanentCity As String = reader("Pcity").ToString()
                permPinCode.Text = reader("Ppincode").ToString()

                ' Populate Permanent Dropdowns if Null or Empty
                If String.IsNullOrWhiteSpace(permanentCountry) Then
                    PopulateCountries2()
                Else
                    permCountry.SelectedValue = permanentCountry
                End If
                PopulateStates(permanentCountry, permState)
                If String.IsNullOrWhiteSpace(permanentState) Then
                    permState.Items.Clear()
                    permState.Items.Insert(0, New ListItem("--Select State--", "0"))
                Else
                    permState.SelectedValue = permanentState
                End If
                PopulateCities(permanentState, permCity)
                If String.IsNullOrWhiteSpace(permanentCity) Then
                    permCity.Items.Clear()
                    permCity.Items.Insert(0, New ListItem("--Select City--", "0"))
                Else
                    permCity.SelectedValue = permanentCity
                End If
            Else
                ' If no data is found, populate dropdowns
                PopulateCountries()
                PopulateCountries2()
            End If
        End Using
    End Sub




    Protected Sub ContactSaveButton_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SaveButton.Click
        ' Get the UserName from session
        Dim userName As String = ViewState("UserName").ToString()


        Using conn As New SqlConnection(connStr)
            conn.Open()

            ' Check if the record exists
            Dim checkCmd As New SqlCommand("SELECT COUNT(*) FROM sturegistration WHERE Student = @Student", conn)
            checkCmd.Parameters.AddWithValue("@Student", userName)
            Dim recordCount As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

            If recordCount > 0 Then
                ' Update existing record
                Dim updateCmd As New SqlCommand("UPDATE sturegistration SET Radd = @Radd, Rcountry = @Rcountry, RState = @RState, Rcity = @Rcity, Rpincode = @Rpincode, Padd = @Padd, Pcountry = @Pcountry, PState = @PState, Pcity = @Pcity, Ppincode = @Ppincode WHERE Student = @Student", conn)
                updateCmd.Parameters.AddWithValue("@Radd", resAddress.Text)
                updateCmd.Parameters.AddWithValue("@Rcountry", resCountry.SelectedValue)
                updateCmd.Parameters.AddWithValue("@RState", resState.SelectedValue)
                updateCmd.Parameters.AddWithValue("@Rcity", resCity.SelectedValue)
                updateCmd.Parameters.AddWithValue("@Rpincode", resPinCode.Text)
                updateCmd.Parameters.AddWithValue("@Padd", permAddress.Text)
                updateCmd.Parameters.AddWithValue("@Pcountry", permCountry.SelectedValue)
                updateCmd.Parameters.AddWithValue("@PState", permState.SelectedValue)
                updateCmd.Parameters.AddWithValue("@Pcity", permCity.SelectedValue)
                updateCmd.Parameters.AddWithValue("@Ppincode", permPinCode.Text)
                updateCmd.Parameters.AddWithValue("@Student", userName)
                updateCmd.ExecuteNonQuery()
            Else
                ' Insert new record
                Dim insertCmd As New SqlCommand("INSERT INTO sturegistration (Student, Radd, Rcountry, RState, Rcity, Rpincode, Padd, Pcountry, PState, Pcity, Ppincode) VALUES (@Student, @Radd, @Rcountry, @RState, @Rcity, @Rpincode, @Padd, @Pcountry, @PState, @Pcity, @Ppincode)", conn)
                insertCmd.Parameters.AddWithValue("@Student", userName)
                insertCmd.Parameters.AddWithValue("@Radd", resAddress.Text)
                insertCmd.Parameters.AddWithValue("@Rcountry", resCountry.SelectedValue)
                insertCmd.Parameters.AddWithValue("@RState", resState.SelectedValue)
                insertCmd.Parameters.AddWithValue("@Rcity", resCity.SelectedValue)
                insertCmd.Parameters.AddWithValue("@Rpincode", resPinCode.Text)
                insertCmd.Parameters.AddWithValue("@Padd", permAddress.Text)
                insertCmd.Parameters.AddWithValue("@Pcountry", permCountry.SelectedValue)
                insertCmd.Parameters.AddWithValue("@PState", permState.SelectedValue)
                insertCmd.Parameters.AddWithValue("@Pcity", permCity.SelectedValue)
                insertCmd.Parameters.AddWithValue("@Ppincode", permPinCode.Text)
                insertCmd.ExecuteNonQuery()
            End If
        End Using
        Response.Redirect("ApplicationForm.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                         "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                         "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                         "&tab=" & Server.UrlEncode("3"))
    End Sub

    Protected Sub resCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        PopulateStates(resCountry.SelectedValue, resState)
    End Sub
    Protected Sub resState_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        PopulateCities(resState.SelectedValue, resCity)
    End Sub
    Protected Sub chkSameAddress_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        If chkSameAddress.Checked Then
            ' Copy Residential Address to Permanent Address
            permAddress.Text = resAddress.Text
            permCountry.SelectedValue = resCountry.SelectedValue
            PopulateStates(resCountry.SelectedValue, permState)
            permState.SelectedValue = resState.SelectedValue

            PopulateCities(permState.SelectedValue, permCity)
            permCity.SelectedValue = resCity.SelectedValue
            permPinCode.Text = resPinCode.Text

            ' Make Permanent Address Fields Read-Only
            permAddress.ReadOnly = True
            permCountry.Enabled = False
            permState.Enabled = False
            permCity.Enabled = False
            permPinCode.ReadOnly = True
        Else
            ' Clear Permanent Address Fields and Make Editable
            permAddress.Text = String.Empty
            permCountry.SelectedIndex = 0
            permState.Items.Clear()
            permState.Items.Insert(0, New ListItem("--Select State--", "0"))
            permCity.Items.Clear()
            permCity.Items.Insert(0, New ListItem("--Select City--", "0"))
            permPinCode.Text = String.Empty

            ' Enable Editing for Permanent Address Fields
            permAddress.ReadOnly = False
            permCountry.Enabled = True
            permState.Enabled = True
            permCity.Enabled = True
            permPinCode.ReadOnly = False
        End If
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
                            PhotoPreview.ImageUrl = filePath
                            photoFound = True
                        Else
                            PhotoPreview.ImageUrl = "~/images/user.png"
                        End If
                    ElseIf docID = 2 Then
                        ' Signature
                        If File.Exists(Server.MapPath(filePath)) Then
                            SignaturePreview.ImageUrl = filePath
                            signatureFound = True
                        Else
                            SignaturePreview.ImageUrl = "~/images/user.png"
                        End If
                    End If
                End While

                ' If no records exist, set default images
                If Not photoFound Then
                    PhotoPreview.ImageUrl = "~/images/user.png"
                End If

                If Not signatureFound Then
                    SignaturePreview.ImageUrl = "~/images/user.png"
                End If
            End Using
        End Using

        ' Set HiddenPhotoSignStatus based on availability of both photo and signature
        If photoFound AndAlso signatureFound Then
            HiddenPhotoSignStatus.Value = "1"
        Else
            HiddenPhotoSignStatus.Value = "0"
        End If
    End Sub



    Protected Sub btnPhotoSignSave_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim studentID As String = ViewState("UserID")

        ' Folder path to save images
        Dim folderPath As String = Server.MapPath("~/images/")
        Dim photoFileName As String = Nothing
        Dim signFileName As String = Nothing

        ' Check if the folder exists, if not, create it
        If Not Directory.Exists(folderPath) Then
            Directory.CreateDirectory(folderPath)
        End If

        ' Upload photo if a file is selected
        If PhotoUpload.HasFile Then
            photoFileName = studentID & "_photo" & Path.GetExtension(PhotoUpload.FileName)
            PhotoUpload.SaveAs(Path.Combine(folderPath, photoFileName))
        End If

        ' Upload signature if a file is selected
        If SignatureUpload.HasFile Then
            signFileName = studentID & "_sign" & Path.GetExtension(SignatureUpload.FileName)
            SignatureUpload.SaveAs(Path.Combine(folderPath, signFileName))
        End If

        ' Database connection to update/insert photo or signature
        Using conn As New SqlConnection(connStr)
            conn.Open()

            ' If photo is uploaded, insert or update the record for the photo
            If Not String.IsNullOrEmpty(photoFileName) Then
                Dim insertPhotoQuery As String = "IF EXISTS (SELECT 1 FROM StudentEssentialDoc WHERE StudentID = @StudentID AND EssentialDocID = 1) " & _
    "BEGIN " & _
    "    UPDATE StudentEssentialDoc " & _
    "    SET Dated = GETDATE(), Docxpath = @Photo " & _
    "    WHERE StudentID = @StudentID AND EssentialDocID = 1 " & _
    "END " & _
    "ELSE " & _
    "BEGIN " & _
    "    INSERT INTO StudentEssentialDoc (Dated, StudentID, EssentialDocID, Docxpath) " & _
    "    VALUES (GETDATE(), @StudentID, 1, @Photo) " & _
    "END"

                Using cmd As New SqlCommand(insertPhotoQuery, conn)
                    cmd.Parameters.AddWithValue("@Photo", photoFileName)
                    cmd.Parameters.AddWithValue("@StudentID", studentID)
                    cmd.ExecuteNonQuery()
                End Using
            End If

            ' If signature is uploaded, insert or update the record for the signature
            If Not String.IsNullOrEmpty(signFileName) Then
                Dim insertSignQuery As String = "IF EXISTS (SELECT 1 FROM StudentEssentialDoc WHERE StudentID = @StudentID AND EssentialDocID = 2) " & _
    "BEGIN " & _
    "    UPDATE StudentEssentialDoc " & _
    "    SET Dated = GETDATE(), Docxpath = @Sign " & _
    "    WHERE StudentID = @StudentID AND EssentialDocID = 2 " & _
    "END " & _
    "ELSE " & _
    "BEGIN " & _
    "    INSERT INTO StudentEssentialDoc (Dated, StudentID, EssentialDocID, Docxpath) " & _
    "    VALUES (GETDATE(), @StudentID, 2, @Sign) " & _
    "END"

                Using cmd As New SqlCommand(insertSignQuery, conn)
                    cmd.Parameters.AddWithValue("@Sign", signFileName)
                    cmd.Parameters.AddWithValue("@StudentID", studentID)
                    cmd.ExecuteNonQuery()
                End Using
            End If
        End Using

        ' Reload the images to reflect changes on the frontend
        LoadStudentImages(studentID)

        ' Redirect to ApplicationForm.aspx?tab=2 after saving
        Response.Redirect("ApplicationForm.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                         "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                         "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                         "&tab=" & Server.UrlEncode("2"))
    End Sub


    Protected Sub LoadEducationData()
        Dim userID As String = ViewState("UserID").ToString()

        ' Fetch data for current UserID
        Using conn As New SqlConnection(connStr)
            conn.Open()
            Dim query As String = "SELECT e.Qualification AS ExamName, e.Institution AS InstituteName, e.Roll_No AS RollNumber, e.PassingYear, b.Name AS BoardID, e.Marks AS ObtainedMarks, e.MaxMarks AS MaxMarks FROM StudentEducation e JOIN boarduniversity b ON b.ID = e.Board WHERE StudentID = @StudentID"
            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@StudentID", userID)

            Dim dt As New DataTable()
            Dim adapter As New SqlDataAdapter(cmd)
            adapter.Fill(dt)

            ' Check if any rows were found
            If dt.Rows.Count > 0 Then
                hiddenEducationStatus.Value = "1"
            Else
                hiddenEducationStatus.Value = "0"
            End If

            ' Add the Percentage column to DataTable
            dt.Columns.Add("Percentage", GetType(Decimal))

            ' Calculate percentage and add it to the DataTable
            For Each row As DataRow In dt.Rows
                Dim obtainedMarks As Decimal = Convert.ToDecimal(row("ObtainedMarks"))
                Dim maxMarks As Decimal = Convert.ToDecimal(row("MaxMarks"))
                Dim percentage As Decimal = 0

                If maxMarks > 0 Then
                    percentage = (obtainedMarks * 100D / maxMarks)
                End If

                row("Percentage") = Math.Round(percentage, 2)
            Next

            ' Bind the DataTable to the GridView
            gvEducation.DataSource = dt
            gvEducation.DataBind()

            ' Get the list of existing qualifications for the current user
            Dim existingQualifications As New List(Of String)()
            For Each row As DataRow In dt.Rows
                existingQualifications.Add(row("ExamName").ToString())
            Next

            ' Populate the dropdown, excluding existing qualifications
            Dim allQualifications As String() = {"High School", "Diploma", "Intermediate", "Graduation", "Post Graduation", "Other"}
            ddlExamName.Items.Clear()

            For Each qualification As String In allQualifications
                If Not existingQualifications.Contains(qualification) Then
                    ddlExamName.Items.Add(New ListItem(qualification))
                End If
            Next
        End Using
    End Sub


    Private Sub BindBoardDropdown()
        Using conn As New SqlConnection(connStr)
            conn.Open()
            Dim query As String = "SELECT ID, Name FROM boarduniversity"
            Dim cmd As New SqlCommand(query, conn)
            Dim dt As New DataTable()
            Dim adapter As New SqlDataAdapter(cmd)
            adapter.Fill(dt)

            ddlBoard.DataSource = dt
            ddlBoard.DataTextField = "Name"
            ddlBoard.DataValueField = "ID"
            ddlBoard.DataBind()

            ddlBoard.Items.Insert(0, New ListItem("Select Board/University", ""))
        End Using
    End Sub

    Private Sub PopulateYearDropDown()
        ' Clear existing items in case of PostBack
        ddlYear.Items.Clear()

        ' Add default "Select Year" option
        ddlYear.Items.Add(New ListItem("Select Year", ""))

        ' Get the current year
        Dim currentYear As Integer = DateTime.Now.Year

        ' Loop from the current year to 1900 and add each year to the DropDownList
        For year As Integer = currentYear To 1900 Step -1
            ddlYear.Items.Add(New ListItem(year.ToString(), year.ToString()))
        Next
    End Sub

    Private Sub DeleteEducation(ByVal examName As String)
        ' Database connection and deletion logic
        Using conn As New SqlConnection(connStr)
            conn.Open()
            Dim query As String = "DELETE FROM StudentEducation WHERE Qualification = @ExamName"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@ExamName", examName)
                cmd.ExecuteNonQuery()
            End Using
        End Using
        Response.Redirect("ApplicationForm.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                         "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                         "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                         "&tab=" & Server.UrlEncode("1"))
    End Sub


Protected Sub gvEducation_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        If e.CommandName = "DeleteRow" Then
            ' Retrieve the ExamName from the CommandArgument
            Dim examName As String = e.CommandArgument.ToString()
            ' Call the delete method
            DeleteEducation(examName)
            ' Reload the grid data after deletion
            LoadEducationData()

            lblStatusMessage.Text = "Education record deleted successfully!"
            lblStatusMessage.CssClass = "success"
        End If
    End Sub

    Protected Sub ShowForm(ByVal sender As Object, ByVal e As EventArgs)
        ' Display the form when "Add More Education" is clicked
        addMoreForm.Style("display") = "block"
    End Sub
    Protected Sub SaveEducation(ByVal sender As Object, ByVal e As EventArgs)
        Dim userID As String = ViewState("UserID").ToString()

        ' Ensure fields are filled
        If String.IsNullOrEmpty(ddlExamName.SelectedValue) OrElse String.IsNullOrEmpty(txtInstituteName.Text) OrElse
           String.IsNullOrEmpty(txtRollNumber.Text) OrElse String.IsNullOrEmpty(ddlYear.SelectedValue) OrElse
           String.IsNullOrEmpty(txtObtainedMarks.Text) OrElse
           String.IsNullOrEmpty(txtMaxMarks.Text) Then
            lblStatusMessage.Text = "Please fill all details."
            lblStatusMessage.CssClass = "error"
            Return
        End If
        If String.IsNullOrEmpty(ddlBoard.SelectedValue) Then
            lblStatusMessage.Text = "Please select a Board/University."
            lblStatusMessage.CssClass = "error"
            Return
        End If

        ' Remaining logic remains unchanged
        Using conn As New SqlConnection(connStr)
            conn.Open()

            ' Check if the entry already exists
            Dim checkQuery As String = "SELECT COUNT(*) FROM StudentEducation WHERE StudentID = @StudentID AND Qualification = @Qualification AND Roll_No = @RollNumber AND PassingYear = @PassingYear"
            Dim checkCmd As New SqlCommand(checkQuery, conn)
            checkCmd.Parameters.AddWithValue("@StudentID", userID)
            checkCmd.Parameters.AddWithValue("@Qualification", ddlExamName.SelectedValue)
            checkCmd.Parameters.AddWithValue("@RollNumber", txtRollNumber.Text)
            checkCmd.Parameters.AddWithValue("@PassingYear", ddlYear.SelectedValue)

            Dim recordExists As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

            If recordExists > 0 Then
                lblStatusMessage.Text = "This qualification is already added."
                lblStatusMessage.CssClass = "error"
                Response.Redirect("ApplicationForm.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                         "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                         "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                         "&tab=" & Server.UrlEncode("1"))
                Return
            End If

            ' Calculate percentage
            Dim obtainedMarks As Decimal = Decimal.Parse(txtObtainedMarks.Text)
            Dim maxMarks As Decimal = Decimal.Parse(txtMaxMarks.Text)
            Dim percentage As Decimal = Math.Round((obtainedMarks / maxMarks) * 100, 2)


            ' Insert the new record
            Dim insertQuery As String = "INSERT INTO StudentEducation (StudentID, Qualification, Institution, Roll_No, PassingYear, Board, Marks, MaxMarks, Percentage) VALUES (@StudentID, @Qualification, @InstituteName, @RollNumber, @PassingYear, @BoardID, @ObtainedMarks, @MaxMarks, @Percentage)"
            Dim insertCmd As New SqlCommand(insertQuery, conn)
            insertCmd.Parameters.AddWithValue("@StudentID", userID)
            insertCmd.Parameters.AddWithValue("@Qualification", ddlExamName.SelectedValue)
            insertCmd.Parameters.AddWithValue("@InstituteName", txtInstituteName.Text)
            insertCmd.Parameters.AddWithValue("@RollNumber", txtRollNumber.Text)
            insertCmd.Parameters.AddWithValue("@PassingYear", ddlYear.SelectedValue)
            insertCmd.Parameters.AddWithValue("@BoardID", ddlBoard.SelectedValue)
            insertCmd.Parameters.AddWithValue("@ObtainedMarks", obtainedMarks)
            insertCmd.Parameters.AddWithValue("@MaxMarks", maxMarks)
            insertCmd.Parameters.AddWithValue("@Percentage", percentage)

            insertCmd.ExecuteNonQuery()
        End Using

        ' Clear and reload
        ClearForm()
        LoadEducationData()
        addMoreForm.Style("display") = "none"

        lblStatusMessage.Text = "Education details saved successfully!"
        lblStatusMessage.CssClass = "success"
        Response.Redirect("ApplicationForm.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                         "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                         "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                         "&tab=" & Server.UrlEncode("1"))
    End Sub


    Protected Sub ClearForm()
        ddlExamName.SelectedIndex = 0
        txtInstituteName.Text = ""
        txtRollNumber.Text = ""
        ddlYear.SelectedIndex = 0
        ddlBoard.SelectedIndex = 0
        txtObtainedMarks.Text = ""
        txtMaxMarks.Text = ""
    End Sub




    Private Sub LoadPersonalDetails(ByVal email As String)
        Using conn As New SqlConnection(connStr)
            Dim cmd As New SqlCommand("SELECT * FROM StuRegistration WHERE Email = @Email", conn)
            cmd.Parameters.AddWithValue("@Email", email)
            conn.Open()
            Dim reader As SqlDataReader = cmd.ExecuteReader()

            Dim isAllFieldsFilled As Boolean = True

            If reader.Read() Then
                ' Check if each field is populated and update the fields accordingly
                Dim firstName As String = reader("FirstName").ToString()
                Dim mobileNo As String = reader("MobileNo").ToString()
                Dim applyCourse As String = reader("Applycourse").ToString()
                Dim gender As String = reader("Gender").ToString()
                Dim dob As Object = reader("DOB")
                Dim fatherName As String = reader("FatherName").ToString()
                Dim motherName As String = reader("MotherName").ToString()
                Dim guardianMobile As String = reader("GuardianMobile").ToString()
                Dim religion As Object = reader("Religion")
                Dim category As Object = reader("Castcategories")

                ' Set fields from the database
                txtFullName.Text = firstName
                txtMobile.Text = mobileNo
                txtApplyFor.Text = applyCourse
                ddlGender.SelectedValue = gender

                If Not IsDBNull(dob) Then
                    txtDOB.Text = Convert.ToDateTime(dob).ToString("yyyy-MM-dd")
                End If

                txtFatherName.Text = fatherName
                txtMotherName.Text = motherName
                txtGuardianMobile.Text = guardianMobile

                ' Set Religion safely
                If Not IsDBNull(religion) Then
                    Dim religionValue As String = religion.ToString()
                    If ddlReligion.Items.FindByValue(religionValue) IsNot Nothing Then
                        ddlReligion.SelectedValue = religionValue
                    Else
                        ddlReligion.SelectedIndex = 0 ' Default to "Select Religion"
                    End If
                End If

                ' Set Category safely
                If Not IsDBNull(category) Then
                    Dim categoryValue As String = category.ToString()
                    If ddlCategory.Items.FindByValue(categoryValue) IsNot Nothing Then
                        ddlCategory.SelectedValue = categoryValue
                    Else
                        ddlCategory.SelectedIndex = 0 ' Default to "Select Category"
                    End If
                End If

                ' Optionally handle SubCategory if necessary
                ' If Not IsDBNull(reader("SubCategory")) Then
                '    ddlSubCategory.SelectedValue = reader("SubCategory").ToString()
                ' End If

                ' Check if all fields are populated
                If String.IsNullOrEmpty(firstName) OrElse
                   String.IsNullOrEmpty(mobileNo) OrElse
                   String.IsNullOrEmpty(applyCourse) OrElse
                   String.IsNullOrEmpty(gender) OrElse
                   String.IsNullOrEmpty(fatherName) OrElse
                   String.IsNullOrEmpty(motherName) OrElse
                   String.IsNullOrEmpty(guardianMobile) OrElse
                   IsDBNull(religion) OrElse
                   IsDBNull(category) Then
                    isAllFieldsFilled = False
                End If
            Else
                isAllFieldsFilled = False
            End If

            ' Set the hidden field value based on whether all fields are filled
            hfFormStatus.Value = If(isAllFieldsFilled, "1", "0")
        End Using
    End Sub


    ' Load Religion from Religion Table
    Private Sub LoadReligion()
        Using conn As New SqlConnection(connStr)
            Dim cmd As New SqlCommand("SELECT ID, Name FROM Religion", conn)
            conn.Open()
            ddlReligion.DataSource = cmd.ExecuteReader()
            ddlReligion.DataTextField = "Name" ' Display text
            ddlReligion.DataValueField = "ID" ' Actual value
            ddlReligion.DataBind()
        End Using
        ddlReligion.Items.Insert(0, New ListItem("Select Religion", ""))
    End Sub


    ' Load Category from Seat Table
    Private Sub LoadCategory()
        Using conn As New SqlConnection(connStr)
            Dim cmd As New SqlCommand("SELECT Seat, SeatID FROM Seat", conn)
            conn.Open()
            ddlCategory.DataSource = cmd.ExecuteReader()
            ddlCategory.DataTextField = "Seat"
            ddlCategory.DataValueField = "SeatID"
            ddlCategory.DataBind()
        End Using
        ddlCategory.Items.Insert(0, New ListItem("Select Category", ""))
    End Sub
    Protected Sub btnPersonalNext_Click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("ApplicationForm.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                          "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                          "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                          "&tab=" & Server.UrlEncode("1"))
    End Sub

    Protected Sub btnEducationNext_Click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("ApplicationForm.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                          "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                          "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                          "&tab=" & Server.UrlEncode("2"))
    End Sub

    Protected Sub btnEducationPrev_Click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("ApplicationForm.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                          "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                          "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                          "&tab=" & Server.UrlEncode("0"))
    End Sub

    Protected Sub btnPhotoSignPrev_Click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("ApplicationForm.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                          "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                          "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                          "&tab=" & Server.UrlEncode("1"))
    End Sub

    Protected Sub btnPhotoSignNext_Click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("ApplicationForm.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                          "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                          "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                          "&tab=" & Server.UrlEncode("3"))
    End Sub

    Protected Sub btnContactPrev_Click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("ApplicationForm.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                          "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                          "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                          "&tab=" & Server.UrlEncode("2"))
    End Sub

    Protected Sub btnContactNext_Click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("ApplicationForm.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                          "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                          "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                          "&tab=" & Server.UrlEncode("4"))
    End Sub

    Protected Sub btnDocumentPrev_Click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("ApplicationForm.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                          "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                          "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                          "&tab=" & Server.UrlEncode("3"))
    End Sub

    Protected Sub btnDocumentNext_Click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("ApplicationForm.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                          "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                          "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                          "&tab=" & Server.UrlEncode("5"))
    End Sub

    Protected Sub btnPaymentPrev_Click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("ApplicationForm.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                          "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                          "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                          "&tab=" & Server.UrlEncode("4"))
    End Sub

    ' Save or Update Personal Details on button click
    Protected Sub btnPersonalDetailsSave_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim isValid As Boolean = True

        ' Validation Logic
        genderL.Visible = String.IsNullOrEmpty(ddlGender.SelectedValue)
        If genderL.Visible Then isValid = False

        dobL.Visible = String.IsNullOrEmpty(txtDOB.Text)
        If dobL.Visible Then isValid = False

        fatherL.Visible = String.IsNullOrEmpty(txtFatherName.Text)
        If fatherL.Visible Then isValid = False

        motherL.Visible = String.IsNullOrEmpty(txtMotherName.Text)
        If motherL.Visible Then isValid = False

        gMobileL.Visible = String.IsNullOrEmpty(txtGuardianMobile.Text)
        If gMobileL.Visible Then isValid = False

        religionL.Visible = String.IsNullOrEmpty(ddlReligion.SelectedValue)
        If religionL.Visible Then isValid = False

        categoryL.Visible = String.IsNullOrEmpty(ddlCategory.SelectedValue)
        If categoryL.Visible Then isValid = False

        ' Stop execution if validation fails
        If Not isValid Then Exit Sub

        ' Database Logic
        Using conn As New SqlConnection(connStr)
            conn.Open()

            ' Check if record exists
            Dim checkCmd As New SqlCommand("SELECT COUNT(*) FROM StuRegistration WHERE Email = @Email", conn)
            checkCmd.Parameters.AddWithValue("@Email", txtEmail.Text)
            Dim recordExists As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

            ' Prepare Insert/Update command
            Dim query As String
            If recordExists > 0 Then
                query = "UPDATE StuRegistration SET FirstName = @FullName, MobileNo = @MobileNo, Applycourse = @ApplyFor, Gender = @Gender, DOB = @DOB, FatherName = @FatherName, MotherName = @MotherName, Religion = @Religion, Seatid = @Seatid, GuardianMobile = @GuardianMobile, GuardianIncome = @GuardianIncome, Castcategories = @Castcategories WHERE Email = @Email"
            Else
                query = "INSERT INTO StuRegistration (FirstName, MobileNo, Applycourse, Gender, DOB, FatherName, MotherName, Religion, Seatid, GuardianMobile, GuardianIncome, Email, Castcategories) VALUES (@FullName, @MobileNo, @ApplyFor, @Gender, @DOB, @FatherName, @MotherName, @Religion, @Seatid, @GuardianMobile, @GuardianIncome, @Email, @Castcategories)"
            End If

            Dim cmd As New SqlCommand(query, conn)
            cmd.Parameters.AddWithValue("@FullName", txtFullName.Text)
            cmd.Parameters.AddWithValue("@MobileNo", txtMobile.Text)
            cmd.Parameters.AddWithValue("@ApplyFor", txtApplyFor.Text)
            cmd.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue)
            cmd.Parameters.AddWithValue("@DOB", txtDOB.Text)
            cmd.Parameters.AddWithValue("@FatherName", txtFatherName.Text)
            cmd.Parameters.AddWithValue("@MotherName", txtMotherName.Text)
            cmd.Parameters.AddWithValue("@GuardianMobile", txtGuardianMobile.Text)
            cmd.Parameters.AddWithValue("@Religion", ddlReligion.SelectedValue)
            cmd.Parameters.AddWithValue("@Seatid", ddlCategory.SelectedValue)
            cmd.Parameters.AddWithValue("@Castcategories", ddlCategory.SelectedItem.Text)
            cmd.Parameters.AddWithValue("@GuardianIncome", ddlGuardianIncome.SelectedValue)
            cmd.Parameters.AddWithValue("@Email", txtEmail.Text)

            ' Execute Command
            cmd.ExecuteNonQuery()
        End Using

        ' Redirect after success
        Response.Redirect("ApplicationForm.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                         "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                         "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                         "&tab=" & Server.UrlEncode("0"))
    End Sub

End Class
