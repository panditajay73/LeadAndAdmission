Imports System.Web.Script.Serialization
Imports System.Data.SqlClient
Imports System.IO
Imports System.Data

Partial Class UserProfilePage
    Inherits System.Web.UI.Page
    Private connStr As String = ConfigurationManager.ConnectionStrings("myconnection").ConnectionString
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ViewState("UserID") = Request.QueryString("UserID")
        ViewState("UserName") = Request.QueryString("UserName")
        ViewState("Email") = Request.QueryString("Email")
        If Not IsPostBack Then


            ' Ensure user is logged in
            If Request.QueryString("Email") Is Nothing OrElse Request.QueryString("UserID") Is Nothing OrElse Request.QueryString("UserName") Is Nothing Then
                ' Redirect to login page
                Response.Redirect("login.aspx")
            End If

            LoadPersonalDetails(ViewState("UserID").ToString())
            LoadReligion()
            LoadCategory()

            Dim studentID As String = ViewState("UserID")
            LoadStudentImages(studentID)
            userIcon.ImageUrl = GetUserPhoto(studentID)


            LoadEducationData()
            Dim userName As String = ViewState("UserName") ' Assuming you store the username in session
            PopulateYearDropDown()
            PopulateCountries()
            PopulateCountries2()
            LoadAddressData(ViewState("UserID").ToString())
            ' Call the function to calculate and update the profile completion percentage
            UpdateProfileStatusPercent(userName)
            Dim userID As String = ViewState("UserID")

            Dim email As String = ViewState("Email")

            ' Construct the query string
            Dim queryString As String = "?UserID=" & userID & "&UserName=" & userName & "&Email=" & email

            ' Update links with query parameters
            dashboardLink.HRef = "UserDashboard.aspx" & queryString
            fillApplicationButton.HRef = "ApplicationForm.aspx" & queryString  ' Keep as it is since no navigation link is specified
            statusLink.HRef = "ApplicationStatus.aspx" & queryString
            forumLink.HRef = "DiscussionForum.aspx" & queryString
            previewLink.HRef = "ApplicationFormPreview.aspx" & queryString
            profileLink.HRef = "UserProfilePage.aspx" & queryString
            profileLink2.HRef = "UserProfilePage.aspx" & queryString
            changePass.HRef = "ChangePassword.aspx" & queryString

            BindBoardDropdown()
        End If
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

    Protected Sub btnPersonalNext_Click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("UserProfilePage.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                   "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                   "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                   "&tab=1")
    End Sub
    Protected Sub btnEducationNext_Click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("UserProfilePage.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                          "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                          "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                          "&tab=" & Server.UrlEncode("2"))
    End Sub

    Protected Sub btnEducationPrev_Click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("UserProfilePage.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                          "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                          "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                          "&tab=" & Server.UrlEncode("0"))
    End Sub

    Protected Sub btnPhotoSignPrev_Click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("UserProfilePage.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                          "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                          "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                          "&tab=" & Server.UrlEncode("1"))
    End Sub

    Protected Sub btnPhotoSignNext_Click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("UserProfilePage.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                          "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                          "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                          "&tab=" & Server.UrlEncode("3"))
    End Sub

    Protected Sub btnContactPrev_Click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("UserProfilePage.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                          "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                          "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                          "&tab=" & Server.UrlEncode("2"))
    End Sub

    ' Populate State Dropdown based on CountryID
    Protected Sub resCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles resCountry.SelectedIndexChanged
        Dim countryID As Integer = Convert.ToInt32(resCountry.SelectedValue)
        If countryID > 0 Then
            Using conn As New SqlConnection(connStr)
                Dim query As String = "SELECT ID, Name FROM StateMaster WHERE CountryID = @CountryID"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@CountryID", countryID)

                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                resState.DataSource = reader
                resState.DataTextField = "Name"
                resState.DataValueField = "ID"
                resState.DataBind()
                conn.Close()
            End Using
        End If
        resState.Items.Insert(0, New ListItem("--Select State--", "0"))
        resCity.Items.Clear()
        resCity.Items.Insert(0, New ListItem("--Select City--", "0"))
    End Sub

    ' Populate City Dropdown based on StateID
    Protected Sub resState_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles resState.SelectedIndexChanged
        Dim stateID As Integer = Convert.ToInt32(resState.SelectedValue)
        If stateID > 0 Then
            Using conn As New SqlConnection(connStr)
                Dim query As String = "SELECT ID, Name FROM CityMaster WHERE StateID = @StateID"
                Dim cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@StateID", stateID)

                conn.Open()
                Dim reader As SqlDataReader = cmd.ExecuteReader()
                resCity.DataSource = reader
                resCity.DataTextField = "Name"
                resCity.DataValueField = "ID"
                resCity.DataBind()
                conn.Close()
            End Using
        End If
        resCity.Items.Insert(0, New ListItem("--Select City--", "0"))
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
    Private Sub UpdateProfileStatusPercent(ByVal userName As String)
        ' Convert hidden field values to integers
        Dim contactStatus As Integer = Convert.ToInt32(HiddenContactStatus.Value)
        Dim photoSignStatus As Integer = Convert.ToInt32(HiddenPhotoSignStatus.Value)
        Dim formStatusProfile As Integer = Convert.ToInt32(hfFormStatusProfile.Value)
        Dim educationStatus As Integer = Convert.ToInt32(hiddenEducationStatus.Value)

        ' Calculate the sum of the statuses
        Dim totalScore As Integer = contactStatus + photoSignStatus + formStatusProfile + educationStatus

        ' Calculate the percentage out of 23
        Dim percentage As Double = (totalScore / 23.0) * 100
        Dim percentageStr As String = Math.Round(percentage, 2).ToString()

        ' Update ProfileStatusPercent in the database
        Using conn As New SqlConnection(connStr)
            Dim query As String = "UPDATE sturegistration SET ProfileStatusPercent = @ProfileStatusPercent WHERE Student = @Student"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@ProfileStatusPercent", percentageStr)
                cmd.Parameters.AddWithValue("@Student", userName)
                conn.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using
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
        Response.Redirect("UserProfilePage.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
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
                Response.Redirect("UserProfilePage.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
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
        Response.Redirect("UserProfilePage.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
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
        Response.Redirect("UserProfilePage.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                          "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                          "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                          "&tab=" & Server.UrlEncode("3"))
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
        Dim photoSignPoints As Integer = 0 ' To hold the points for photo and signature

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
                            photoSignPoints += 1 ' Add 1 point for photo
                        Else
                            PhotoPreview.ImageUrl = "~/images/user.png"
                        End If
                    ElseIf docID = 2 Then
                        ' Signature
                        If File.Exists(Server.MapPath(filePath)) Then
                            SignaturePreview.ImageUrl = filePath
                            signatureFound = True
                            photoSignPoints += 1 ' Add 1 point for signature
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

        ' Update HiddenPhotoSignStatus with total points
        HiddenPhotoSignStatus.Value = photoSignPoints.ToString()
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
        Response.Redirect("UserProfilePage.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                          "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                          "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                          "&tab=" & Server.UrlEncode("2"))
    End Sub


    'Protected Sub btnPhotoSignSave_Click(ByVal sender As Object, ByVal e As EventArgs)
    '    Dim studentID As String = Session("UserID")

    '    ' Folder path to save images
    '    Dim folderPath As String = Server.MapPath("~/images/")
    '    Dim photoFileName As String = studentID & "_photo" & Path.GetExtension(PhotoUpload.FileName)
    '    Dim signFileName As String = studentID & "_sign" & Path.GetExtension(SignatureUpload.FileName)

    '    ' Upload photo if a file is selected
    '    If PhotoUpload.HasFile Then
    '        PhotoUpload.SaveAs(Path.Combine(folderPath, photoFileName))
    '    End If

    '    ' Upload signature if a file is selected
    '    If SignatureUpload.HasFile Then
    '        SignatureUpload.SaveAs(Path.Combine(folderPath, signFileName))
    '    End If

    '    Using conn As New SqlConnection("Data Source=(local);Initial Catalog=College;Integrated Security=True")
    '        conn.Open()

    '        ' Insert or update records for Photo
    '        Dim insertPhotoQuery As String = "INSERT INTO StudentEssentialDoc (Dated, StudentID, EssentialDocID, Docxpath) VALUES (GETDATE(), @StudentID, 1, @Photo)"
    '        Using cmd As New SqlCommand(insertPhotoQuery, conn)
    '            cmd.Parameters.AddWithValue("@Photo", photoFileName)
    '            cmd.Parameters.AddWithValue("@StudentID", studentID)
    '            cmd.ExecuteNonQuery()
    '        End Using

    '        ' Insert or update records for Signature
    '        Dim insertSignQuery As String = "INSERT INTO StudentEssentialDoc (Dated, StudentID, EssentialDocID, Docxpath) VALUES (GETDATE(), @StudentID, 2, @Sign)"
    '        Using cmd As New SqlCommand(insertSignQuery, conn)
    '            cmd.Parameters.AddWithValue("@Sign", signFileName)
    '            cmd.Parameters.AddWithValue("@StudentID", studentID)
    '            cmd.ExecuteNonQuery()
    '        End Using
    '    End Using

    '    ' Reload the images to reflect changes on the frontend
    '    LoadStudentImages(studentID)
    'End Sub

    Private Sub LoadPersonalDetails(ByVal Registrationid As String)
        Using conn As New SqlConnection(connStr)
            Dim cmd As New SqlCommand("SELECT * FROM StuRegistration WHERE Registrationid = @Registrationid", conn)
            cmd.Parameters.AddWithValue("@Registrationid", Registrationid)
            conn.Open()
            Dim reader As SqlDataReader = cmd.ExecuteReader()

            Dim filledFieldCount As Integer = 0

            If reader.Read() Then
                ' Check if each field is populated and update the fields accordingly
                Dim firstName As String = reader("FirstName").ToString()
                Dim mobileNo As String = reader("MobileNo").ToString()
                Dim emailID As String = reader("Email").ToString()
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
                txtEmail.Text = emailID
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

                ' Increment the counter for each filled field
                If Not String.IsNullOrEmpty(firstName) Then filledFieldCount += 1
                If Not String.IsNullOrEmpty(mobileNo) Then filledFieldCount += 1
                If Not String.IsNullOrEmpty(applyCourse) Then filledFieldCount += 1
                If Not String.IsNullOrEmpty(gender) Then filledFieldCount += 1
                If Not IsDBNull(dob) Then filledFieldCount += 1
                If Not String.IsNullOrEmpty(fatherName) Then filledFieldCount += 1
                If Not String.IsNullOrEmpty(motherName) Then filledFieldCount += 1
                If Not String.IsNullOrEmpty(guardianMobile) Then filledFieldCount += 1
                If Not IsDBNull(religion) Then filledFieldCount += 1
                If Not IsDBNull(category) Then filledFieldCount += 1

            End If

            ' Optionally, you can store the total count in a hidden field or display it on the page
            hfFormStatusProfile.Value = filledFieldCount.ToString()
            ' For example, you can display the count somewhere in the UI
            ' lblFilledFieldCount.Text = "Total filled fields: " & filledFieldCount
        End Using
    End Sub

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

    ' Save or Update Personal Details on button click
    Protected Sub btnPersonalDetailsSave_Click(ByVal sender As Object, ByVal e As EventArgs)
        Using conn As New SqlConnection(connStr)
            conn.Open()

            ' Check if the user record exists
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

            ' Show success message
            ' ScriptManager.RegisterStartupScript(Me, Me.GetType(), "alert", "alert('Personal details saved successfully!');", True)
        End Using
        Response.Redirect("UserProfilePage.aspx?UserID=" & Server.UrlEncode(ViewState("UserID").ToString()) & _
                          "&UserName=" & Server.UrlEncode(ViewState("UserName").ToString()) & _
                          "&Email=" & Server.UrlEncode(ViewState("Email").ToString()) & _
                          "&tab=" & Server.UrlEncode("0"))
    End Sub
End Class
