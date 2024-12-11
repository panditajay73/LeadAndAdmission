Imports System.Data.SqlClient
Imports System.Data
Imports System.IO

Partial Class AuthorityPages_StudentDetails
    Inherits System.Web.UI.Page
    Private connStr As String = ConfigurationManager.ConnectionStrings("myconnection").ConnectionString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' Retrieve RegistrationID from the query string
            Dim registrationID As String = Request.QueryString("registrationid")
            If Not String.IsNullOrEmpty(registrationID) Then
                ' Use RegistrationID instead of Session("UserID")
                LoadStudentImages(registrationID)
                LoadPersonalDetails(registrationID) ' Using session email if needed
                LoadEducationData(registrationID)
                LoadAddressData(registrationID) ' Assuming session username is still required
                LoadDocuments(registrationID)
            Else
                ' Handle missing RegistrationID, possibly redirect or show an error
                Response.Redirect("ErrorPage.aspx")
            End If

            If Not String.IsNullOrEmpty(registrationID) Then
                ' Check the current status of the registration
                Dim status As String = GetRegistrationStatus(registrationID)

                ' Update button state based on the current status
                Select Case status
                    Case "AppSubmitted"
                        UserStatusLabel.Text = "New Application!!"
                        UserStatusLabel.ForeColor = Drawing.Color.Green
                    Case "DocVerified"
                        UserStatusLabel.Text = "Document Verified!!"
                        UserStatusLabel.ForeColor = Drawing.Color.Green
                    Case "Verified"
                        UserStatusLabel.Text = "Registration Approved!!"
                        UserStatusLabel.ForeColor = Drawing.Color.Green
                    Case "Approved"
                        UserStatusLabel.Text = "Admission Approved!!"
                        UserStatusLabel.ForeColor = Drawing.Color.Green
                        footerT.Visible = False
                    Case "Pending"
                        UserStatusLabel.Text = "Verification Pending!!"
                        UserStatusLabel.ForeColor = Drawing.Color.Red
                    Case "Rejected"
                        UserStatusLabel.Text = "Application Rejected!!"
                        UserStatusLabel.ForeColor = Drawing.Color.Red
                End Select
            End If
        End If
    End Sub
    Private Function GetRegistrationStatus(ByVal registrationID As String) As String
        Dim status As String = String.Empty
        Dim query As String = "SELECT ApplicationStatus FROM stuRegistration WHERE RegistrationID = @RegistrationID"

        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@RegistrationID", registrationID)
                conn.Open()
                Dim result As Object = cmd.ExecuteScalar()
                If result IsNot Nothing AndAlso Not IsDBNull(result) Then
                    status = result.ToString()
                End If
            End Using
        End Using

        Return status
    End Function
    ' Method to update registration status based on the selected action (Verify/Reject)
    'Private Sub UpdateRegistrationStatus(ByVal isVerify As Boolean)
    '    ' Get the RegistrationID from the query string
    '    Dim registrationID As String = Request.QueryString("registrationid")

    '    If String.IsNullOrEmpty(registrationID) Then
    '        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('RegistrationID is missing.');", True)
    '        Return
    '    End If

    '    ' Open a connection to the database
    '    Using conn As New SqlConnection(connStr)
    '        conn.Open()

    '        ' Get the current ApplicationStatus
    '        Dim currentStatusQuery As String = "SELECT ApplicationStatus FROM stuRegistration WHERE RegistrationID = @RegistrationID"
    '        Dim currentStatus As String = ""

    '        Using checkCmd As New SqlCommand(currentStatusQuery, conn)
    '            checkCmd.Parameters.AddWithValue("@RegistrationID", registrationID)
    '            currentStatus = Convert.ToString(checkCmd.ExecuteScalar())
    '        End Using

    '        ' Initialize variables for the update query
    '        Dim updateQuery As String = ""
    '        Dim status As String = ""
    '        Dim remark As String = ""

    '        If isVerify Then
    '            Select Case currentStatus
    '                Case "AppSubmitted", "Rejected"
    '                    status = "DocVerified"
    '                    remark = "Document Verified"
    '                    updateQuery = "UPDATE stuRegistration SET RegistrationApproved = '1', ApplicationStatus = 'DocVerified', Remark = @Remark WHERE RegistrationID = @RegistrationID"
    '                Case "DocVerified"
    '                    status = "Verified"
    '                    remark = "Registration Verified"
    '                    updateQuery = "UPDATE stuRegistration SET RegistrationApproved = '1', ApplicationStatus = 'Verified', Remark = @Remark WHERE RegistrationID = @RegistrationID"
    '                Case "Verified"
    '                    status = "Approved"
    '                    remark = "Admission Approved"
    '                    updateQuery = "UPDATE stuRegistration SET RegistrationApproved = '1', ApplicationStatus = 'Approved', AdmissionFeeStatus = 'Submitted', AdmissionApproved = 1, Remark = @Remark WHERE RegistrationID = @RegistrationID"
    '                    FetchDetailsAndCallProcedure(registrationID)
    '            End Select
    '        Else
    '            status = "Rejected"
    '            remark = "Application Rejected"
    '            updateQuery = "UPDATE stuRegistration SET RegistrationApproved = '0', ApplicationStatus = 'Rejected', Remark = @Remark WHERE RegistrationID = @RegistrationID"
    '        End If

    '        ' Skip if current status is "Approved"
    '        If currentStatus = "Approved" Then
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('Admission already done for this record.');", True)
    '            Return
    '        End If

    '        ' Execute the update query
    '        If Not String.IsNullOrEmpty(updateQuery) Then
    '            Using cmd As New SqlCommand(updateQuery, conn)
    '                cmd.Parameters.AddWithValue("@Remark", If(String.IsNullOrEmpty(remark), DBNull.Value, remark))
    '                cmd.Parameters.AddWithValue("@RegistrationID", registrationID)
    '                cmd.ExecuteNonQuery()
    '            End Using

    '            ' Provide feedback based on the status
    '            Select Case status
    '                Case "Verified"
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('Registration Approved Successfully.');", True)
    '                Case "DocVerified"
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('Document Verified Successfully.');", True)
    '                Case "Rejected"
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('Application Rejected.');", True)
    '                Case "Approved"
    '                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('Admission Approved Successfully.');", True)
    '            End Select
    '        Else
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('No updates were made.');", True)
    '        End If
    '    End Using
    'End Sub

    Protected Sub btnApprove_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim action As String = ddlAction.SelectedValue
        Dim remark As String = remarks.Text.Trim()
        Dim status As String = ""
        Dim registrationID As String = Request.QueryString("registrationid")

        If String.IsNullOrEmpty(registrationID) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('RegistrationID is missing.');", True)
            Return
        End If

        Using conn As New SqlConnection(connStr)
            conn.Open()

            ' Get the current ApplicationStatus
            Dim currentStatusQuery As String = "SELECT ApplicationStatus FROM stuRegistration WHERE RegistrationID = @RegistrationID"
            Dim currentStatus As String = ""

            Using checkCmd As New SqlCommand(currentStatusQuery, conn)
                checkCmd.Parameters.AddWithValue("@RegistrationID", registrationID)
                currentStatus = Convert.ToString(checkCmd.ExecuteScalar())
            End Using

            ' Determine the new status and update query
            Dim updateQuery As String = ""
            If action = "Verify" Then
                Select Case currentStatus
                    Case "AppSubmitted", "Pending"
                        status = "DocVerified"
                        UserStatusLabel.Text = "Document Verified!!"
                        UserStatusLabel.ForeColor = Drawing.Color.Green
                        updateQuery = "UPDATE stuRegistration SET RegistrationApproved = '1', ApplicationStatus = 'DocVerified', Remark = @Remark WHERE RegistrationID = @RegistrationID"
                    Case "DocVerified"
                        status = "Verified"
                        UserStatusLabel.Text = "Registration Approved!!"
                        UserStatusLabel.ForeColor = Drawing.Color.Green
                        updateQuery = "UPDATE stuRegistration SET RegistrationApproved = '1', ApplicationStatus = 'Verified', Remark = @Remark WHERE RegistrationID = @RegistrationID"
                    Case "Verified"
                        status = "Approved"
                        UserStatusLabel.Text = "Application Approved!!"
                        UserStatusLabel.ForeColor = Drawing.Color.Green
                        updateQuery = "UPDATE stuRegistration SET RegistrationApproved = '1', ApplicationStatus = 'Approved', AdmissionFeeStatus = 'Submitted', AdmissionApproved = 1, Remark = @Remark WHERE RegistrationID = @RegistrationID"
                        FetchDetailsAndCallProcedure(registrationID)
                End Select
            ElseIf action = "Reject" Then
                status = "Rejected"
                UserStatusLabel.Text = "Application Rejected!!"
                UserStatusLabel.ForeColor = Drawing.Color.Red
                updateQuery = "UPDATE stuRegistration SET RegistrationApproved = '0', ApplicationStatus = 'Rejected', Remark = @Remark WHERE RegistrationID = @RegistrationID"
            ElseIf action = "Update" Then
                status = "Pending"
                UserStatusLabel.Text = "Application in pending!!"
                UserStatusLabel.ForeColor = Drawing.Color.Red
                updateQuery = "UPDATE stuRegistration SET ApplicationStatus = 'Pending', Remark = @Remark WHERE RegistrationID = @RegistrationID"
            End If

            ' Skip if current status is "Approved"
            If currentStatus = "Approved" Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('Admission already done for this record.');", True)
                Return
            End If

            ' Execute the update query
            If Not String.IsNullOrEmpty(updateQuery) Then
                Using cmd As New SqlCommand(updateQuery, conn)
                    cmd.Parameters.AddWithValue("@Remark", If(String.IsNullOrEmpty(remark), DBNull.Value, remark))
                    cmd.Parameters.AddWithValue("@RegistrationID", registrationID)
                    cmd.ExecuteNonQuery()
                End Using

                ' Provide feedback based on the status
                Select Case status
                    Case "Verified"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('Registration Approved Successfully.');", True)
                    Case "DocVerified"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('Document Verified Successfully.');", True)
                    Case "Rejected"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('Application Rejected.');", True)
                    Case "Approved"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('Admission Approved Successfully.');", True)
                    Case "Pending"
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('Application added to the waiting list.');", True)
                End Select
            Else
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('No records were updated.');", True)
            End If

            remarks.Text = ""
        End Using
    End Sub


    Private Sub FetchDetailsAndCallProcedure(ByVal registrationID As String)
        Using conn As New SqlConnection(connStr)
            conn.Open()

            ' Step 1: Fetch details from stuRegistration table
            Dim fetchQuery As String = "SELECT SessionID, AYID, CourseID, Sem, BatchID, SeatID, RegistrationFee FROM stuRegistration WHERE RegistrationID = @RegistrationID"
            Dim sessionID As String = ""
            Dim ayid As String = ""
            Dim courseID As String = ""
            Dim sem As String = ""
            Dim userID As String = "1197"
            Dim batchID As String = ""
            Dim seatID As String = ""
            Dim registrationFee As String = ""

            Using fetchCmd As New SqlCommand(fetchQuery, conn)
                fetchCmd.Parameters.AddWithValue("@RegistrationID", registrationID)

                Using reader As SqlDataReader = fetchCmd.ExecuteReader()
                    If reader.Read() Then
                        sessionID = Convert.ToString(reader("SessionID"))
                        ayid = Convert.ToString(reader("AYID"))
                        courseID = Convert.ToString(reader("CourseID"))
                        sem = Convert.ToString(reader("Sem"))
                        batchID = Convert.ToString(reader("BatchID"))
                        seatID = Convert.ToString(reader("SeatID"))
                        registrationFee = Convert.ToString(reader("RegistrationFee"))
                    End If
                End Using
            End Using

            ' Step 2: Call the stored procedure GetAdmisnno
            Using spCmd As New SqlCommand("GetAdmisnno", conn)
                spCmd.CommandType = CommandType.StoredProcedure
                spCmd.Parameters.AddWithValue("@Sessionid", sessionID)
                spCmd.Parameters.AddWithValue("@ayid", ayid)
                spCmd.Parameters.AddWithValue("@Regisid", registrationID)
                spCmd.Parameters.AddWithValue("@Courseid", courseID)
                spCmd.Parameters.AddWithValue("@Sem", "1")
                spCmd.Parameters.AddWithValue("@Userid", userID)
                spCmd.Parameters.AddWithValue("@batchid", batchID)
                spCmd.Parameters.AddWithValue("@SeatID", seatID)
                spCmd.Parameters.AddWithValue("@registrationfee", registrationFee)
                spCmd.Parameters.AddWithValue("@admissionapproved", "1")

                ' Execute the stored procedure
                spCmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub
    Protected Function GetUserPhoto(ByVal registrationID As String) As String
        Dim defaultPhoto As String = "~/images/user.png"
        Dim photoPath As String = defaultPhoto

        Try
            Using conn As New SqlConnection(connStr)
                Dim query As String = "SELECT Docxpath FROM StudentEssentialDoc WHERE StudentID = @StudentID AND EssentialDocID = 1"
                Using cmd As New SqlCommand(query, conn)
                    cmd.Parameters.AddWithValue("@StudentID", registrationID)
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

    Private Sub LoadDocuments(ByVal registrationID As String)
        Using connection As New SqlConnection(connStr)
            connection.Open()
            Dim query As String = "SELECT Studocxid, Docxname, Docxpath FROM StudentEssentialDoc WHERE StudentID = @StudentID"
            Using command As New SqlCommand(query, connection)
                command.Parameters.AddWithValue("@StudentID", registrationID)
                Using reader As SqlDataReader = command.ExecuteReader()
                    Dim filteredDocs As New List(Of Object)()

                    While reader.Read()
                        Dim docPath As String = reader("Docxpath").ToString().ToLower()
                        If Not (docPath.Contains("photo") Or docPath.Contains("sign")) Then
                            filteredDocs.Add(New With {
                                .DocumentName = reader("Docxname"),
                                .DocumentPath = reader("Docxpath")
                            })
                        End If
                    End While

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

        Dim fullPath As String = Server.MapPath(documentPath)

        If File.Exists(fullPath) Then
            Dim fileName As String = Path.GetFileName(fullPath)
            Dim fileExtension As String = Path.GetExtension(fullPath)

            Dim contentType As String = "application/octet-stream"
            Select Case fileExtension.ToLower()
                Case ".jpg", ".jpeg"
                    contentType = "image/jpeg"
                Case ".png"
                    contentType = "image/png"
                Case ".pdf"
                    contentType = "application/pdf"
            End Select

            Response.Clear()
            Response.ContentType = contentType
            Response.AddHeader("Content-Disposition", "attachment; filename=" & fileName)
            Response.WriteFile(fullPath)
            Response.End()
        Else
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
    Protected Sub LoadEducationData(ByVal registrationID As String)
        Using conn As New SqlConnection(connStr)
            conn.Open()
            Dim query As String = "SELECT s.Qualification AS EducationName, s.Roll_No AS RollNo, s.PassingYear AS YearOfPassing, s.MaxMarks AS TotalMarks, s.Marks AS ObtainedMarks, (s.Marks * 100 / s.MaxMarks) AS Percentage, b.Name AS CollegeName, s.Board AS Board FROM StudentEducation s join boarduniversity b on b.ID = s.Board WHERE StudentID = @StudentID"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@StudentID", registrationID)

                Dim dt As New DataTable()
                Dim adapter As New SqlDataAdapter(cmd)
                adapter.Fill(dt)

                rptEducationDetails.DataSource = dt
                rptEducationDetails.DataBind()
            End Using
        End Using
    End Sub

    Private Sub LoadStudentImages(ByVal registrationID As String)
        Dim photoFound As Boolean = False
        Dim signatureFound As Boolean = False

        Using conn As New SqlConnection(connStr)
            conn.Open()

            Dim query As String = "SELECT EssentialDocID, Docxpath FROM StudentEssentialDoc WHERE StudentID = @StudentID"
            Using cmd As New SqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@StudentID", registrationID)
                Dim reader As SqlDataReader = cmd.ExecuteReader()

                While reader.Read()
                    Dim docID As Integer = Convert.ToInt32(reader("EssentialDocID"))
                    Dim docFileName As String = reader("Docxpath").ToString()
                    Dim filePath As String = "~/images/" & docFileName

                    If docID = 1 Then
                        If File.Exists(Server.MapPath(filePath)) Then
                            imgPhoto.ImageUrl = filePath
                            photoFound = True
                        Else
                            imgPhoto.ImageUrl = "~/images/user.png"
                        End If
                    ElseIf docID = 2 Then
                        If File.Exists(Server.MapPath(filePath)) Then
                            imgSignature.ImageUrl = filePath
                            signatureFound = True
                        Else
                            imgSignature.ImageUrl = "~/images/user.png"
                        End If
                    End If
                End While

                If Not photoFound Then
                    imgPhoto.ImageUrl = "~/images/user.png"
                End If

                If Not signatureFound Then
                    imgSignature.ImageUrl = "~/images/user.png"
                End If
            End Using
        End Using
    End Sub

    Private Sub LoadPersonalDetails(ByVal registrationID As String)
        Using conn As New SqlConnection(connStr)
            Dim cmd As New SqlCommand("SELECT * FROM StuRegistration WHERE RegistrationID = @RegistrationID", conn)
            cmd.Parameters.AddWithValue("@RegistrationID", registrationID)
            conn.Open()
            Dim reader As SqlDataReader = cmd.ExecuteReader()

            If reader.Read() Then
                RegistrationNo.Text = reader("RegistrationID").ToString()
                Name.Text = reader("FirstName").ToString()
                txtEmail.Text = reader("Email").ToString()
                Mobile.Text = reader("MobileNo").ToString()
                ApplyFor.Text = reader("ApplyCourse").ToString()
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
