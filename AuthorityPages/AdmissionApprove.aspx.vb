Imports System.Data.SqlClient
Imports System.Data

Partial Class AuthorityPages_AdmissionApprove
    Inherits System.Web.UI.Page

    Dim connectionString As String = ConfigurationManager.ConnectionStrings("myconnection").ConnectionString

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            PopulateStatus()
            PopulateCourses()
            BindGrid()
            'Else
            '    ' Update visibility based on ViewState on postback
            '    UpdateVisibility()
        End If
    End Sub

    Private Sub UpdateVisibility()
        Dim rowCount As Integer = If(ViewState("RowCount"), 0)

        ' Set JavaScript variable to determine visibility
        ClientScript.RegisterStartupScript(Me.GetType(), "UpdateVisibility", "updateVisibility(" & rowCount & ");", True)
    End Sub

    Private Sub PopulateStatus()
        ' Get the status from the query string
        Dim selectedStatus As String = Request.QueryString("status")

        If Not String.IsNullOrEmpty(selectedStatus) Then
            ddlStatusFilter.SelectedValue = selectedStatus
        End If
    End Sub

    Protected Sub ddlStatusFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlStatusFilter.SelectedIndexChanged
        Dim selectedStatus As String = ddlStatusFilter.SelectedValue
        Dim selectedCourseId As String = ddlCourses.SelectedValue ' Get selected course

        ' Redirect to update the URL with selected status and course
        Response.Redirect(String.Format("AdmissionApprove.aspx?courseid={0}&status={1}", selectedCourseId, selectedStatus))
    End Sub

     Private Sub PopulateCourses()
        Dim selectedCourseID As String = Request.QueryString("courseid")
        Dim query As String = "SELECT CourseID, Course FROM Exam_course"

        Using conn As New SqlConnection(connectionString)
            Dim cmd As New SqlCommand(query, conn)
            conn.Open()

            Dim reader As SqlDataReader = cmd.ExecuteReader()
            ddlCourses.Items.Clear()

            ddlCourses.Items.Add(New ListItem("All Courses", "All"))

            While reader.Read()
                Dim courseName As String = reader("Course").ToString()
                Dim courseID As String = reader("CourseID").ToString()
                Dim listItem As New ListItem(courseName, courseID)

                If selectedCourseID IsNot Nothing AndAlso selectedCourseID = courseID Then
                    listItem.Selected = True
                End If

                ddlCourses.Items.Add(listItem)
            End While
        End Using
    End Sub

    Private Sub BindGrid()
        Dim courseid As String = Request.QueryString("courseid")
        Dim status As String = Request.QueryString("status")

        If String.IsNullOrEmpty(courseid) Then
            courseid = "All"
        End If

        Using conn As New SqlConnection(connectionString)
            Dim query As String = "SELECT sr.RegistrationID, sr.Student, sr.FatherName, sr.MobileNo, sr.Email, sr.Gender, ec.Course, sr.RegistrationApproved, sr.ApplicationStatus " & _
                                  "FROM stuRegistration sr " & _
                                  "INNER JOIN Exam_course ec ON sr.courseid = ec.CourseID " & _
                                  "WHERE sr.ApplicationStatus IS NOT NULL AND sr.ApplicationStatus <> ''"

            ' CourseID condition
            If courseid <> "All" Then
                If IsNumeric(courseid) Then
                    query &= " AND sr.courseid = @courseid"
                Else
                    query &= " AND sr.Applycourse = @coursecode"
                End If
            End If

            ' Status condition
            Select Case status
                Case "NewList"
                    query &= " AND (sr.ApplicationStatus = 'AppSubmitted')"
                Case "DocVerified"
                    query &= " AND (sr.ApplicationStatus = 'DocVerified')"
                Case "Verified"
                    query &= " AND (sr.ApplicationStatus = 'Verified')"
                Case "Approved"
                    query &= " AND sr.ApplicationStatus = 'Approved' "
                Case "Rejected"
                    query &= " AND (sr.ApplicationStatus = 'Rejected')"
            End Select

            ' Create command and add parameters
            Dim cmd As New SqlCommand(query, conn)
            If courseid <> "All" Then
                If IsNumeric(courseid) Then
                    cmd.Parameters.AddWithValue("@courseid", courseid)
                Else
                    cmd.Parameters.AddWithValue("@coursecode", courseid)
                End If
            End If

            ' Execute and bind data
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable()
            da.Fill(dt)

            ' Add Status column with dynamic messages
            dt.Columns.Add("StatusMessage", GetType(String))
            For Each row As DataRow In dt.Rows
                Select Case row("ApplicationStatus").ToString()
                    Case "Pending"
                        row("StatusMessage") = "Document Verification Pending"
                    Case "DocVerified"
                        row("StatusMessage") = "Document Verified"
                    Case "Verified"
                        row("StatusMessage") = "Registration Approved"
                    Case "Approved"
                        row("StatusMessage") = "Admission Approved"
                    Case "Rejected"
                        row("StatusMessage") = "Application Rejected"
                    Case Else
                        row("StatusMessage") = "New Application"
                End Select
            Next

            ' Add dynamic column to GridView
            If GridViewStudents.Columns.Count = 0 OrElse Not GridViewStudents.Columns.Cast(Of DataControlField)().Any(Function(c) c.HeaderText = "Status") Then
                Dim statusColumn As New BoundField()
                statusColumn.DataField = "StatusMessage"
                statusColumn.HeaderText = "Status"
                GridViewStudents.Columns.Add(statusColumn)
            End If

            ' Bind data to GridView
            GridViewStudents.DataSource = dt
            GridViewStudents.DataBind()

            ' Update visibility and row count
            ViewState("RowCount") = dt.Rows.Count
            UpdateVisibility()
        End Using
    End Sub

    Private Sub FetchDetailsAndCallProcedure(ByVal registrationID As String)
        Using conn As New SqlConnection(connectionString)
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
    Protected Sub backbotton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles backbotton.Click
        Response.Redirect("Applications.aspx")
    End Sub
    Protected Sub btnApprove_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnApprove.Click
        Dim action As String = ddlAction.SelectedValue
        Dim remark As String = remarks.Text.Trim()
        Dim rowsUpdated As Integer = 0
        Dim noSelection As Boolean = True
        Dim status As String = ""

        Using conn As New SqlConnection(connectionString)
            conn.Open()

            For Each row As GridViewRow In GridViewStudents.Rows
                Dim checkbox As CheckBox = CType(row.FindControl("rowCheckbox"), CheckBox)

                If checkbox IsNot Nothing AndAlso checkbox.Checked Then
                    noSelection = False
                    Dim registrationID As String = row.Cells(2).Text

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
                            Case "AppSubmitted"
                                status = "DocVerified"
                                updateQuery = "UPDATE stuRegistration SET RegistrationApproved = '1', ApplicationStatus = 'DocVerified', Remark = @Remark WHERE RegistrationID = @RegistrationID"
                            Case "DocVerified"
                                status = "Verified"
                                updateQuery = "UPDATE stuRegistration SET RegistrationApproved = '1', ApplicationStatus = 'Verified', Remark = @Remark WHERE RegistrationID = @RegistrationID"
                            Case "Verified"
                                status = "Approved"
                                updateQuery = "UPDATE stuRegistration SET RegistrationApproved = '1', ApplicationStatus = 'Approved', AdmissionFeeStatus = 'Submitted', AdmissionApproved = 1, Remark = @Remark WHERE RegistrationID = @RegistrationID"
                                FetchDetailsAndCallProcedure(registrationID)
                        End Select
                    ElseIf action = "Reject" Then
                        status = "Rejected"
                        updateQuery = "UPDATE stuRegistration SET RegistrationApproved = '0', ApplicationStatus = 'Rejected', Remark = @Remark WHERE RegistrationID = @RegistrationID"
                    ElseIf action = "Update" Then
                        status = "Pending"
                        updateQuery = "UPDATE stuRegistration SET ApplicationStatus = 'Pending', Remark = @Remark WHERE RegistrationID = @RegistrationID"
                    End If

                    ' Skip if current status is "Approved"
                    If currentStatus = "Approved" Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('Admission already done for this record.');", True)
                        Continue For
                    End If

                    ' Execute the update query
                    If Not String.IsNullOrEmpty(updateQuery) Then
                        Using cmd As New SqlCommand(updateQuery, conn)
                            cmd.Parameters.AddWithValue("@Remark", If(String.IsNullOrEmpty(remark), DBNull.Value, remark))
                            cmd.Parameters.AddWithValue("@RegistrationID", registrationID)
                            cmd.ExecuteNonQuery()
                            rowsUpdated += 1
                        End Using
                    End If
                End If
            Next

            ' Provide feedback based on the last status
            If noSelection Then
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('Nothing Selected, Please select something.');", True)
            ElseIf rowsUpdated > 0 Then
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
            BindGrid()
        End Using
    End Sub

    'Protected Sub btnApprove_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnApprove.Click
    '    ' Get the selected action and remark
    '    Dim action As String = ddlAction.SelectedValue
    '    Dim remark As String = remarks.Text.Trim()
    '    Dim rowsUpdated As Integer = 0 ' Track the number of rows updated
    '    Dim noSelection As Boolean = True ' Track if no checkboxes were selected

    '    ' Open the connection to the database
    '    Using conn As New SqlConnection(connectionString)
    '        conn.Open()

    '        ' Loop through all the rows in the GridView
    '        For Each row As GridViewRow In GridViewStudents.Rows
    '            ' Find the checkbox in the current row
    '            Dim checkbox As CheckBox = CType(row.FindControl("rowCheckbox"), CheckBox)

    '            ' If the checkbox is checked
    '            If checkbox IsNot Nothing AndAlso checkbox.Checked Then
    '                noSelection = False ' At least one row is selected
    '                ' Get the RegistrationID from the bound field (3rd column in this case)
    '                Dim registrationID As String = row.Cells(2).Text ' Adjust index based on the column position

    '                ' Check the current RegistrationApproved status
    '                Dim currentStatusQuery As String = "SELECT ApplicationStatus FROM stuRegistration WHERE RegistrationID = @RegistrationID"
    '                Dim currentStatus As String = ""

    '                Using checkCmd As New SqlCommand(currentStatusQuery, conn)
    '                    checkCmd.Parameters.AddWithValue("@RegistrationID", registrationID)
    '                    currentStatus = Convert.ToString(checkCmd.ExecuteScalar())
    '                End Using

    '                ' Define the update query
    '                Dim updateQuery As String = "UPDATE stuRegistration SET RegistrationApproved = @RegistrationApproved, ApplicationStatus = @ApplicationStatus, Remark = @Remark, AdmissionFeeStatus = @AdmissionFeeStatus WHERE RegistrationID = @RegistrationID"

    '                ' If action is "Verify" and the current status is already "Verified"
    '                'If action = "Verify" AndAlso currentStatus = "Verified" Then
    '                '    ' Update to "Approved" and set AdmissionFeeStatus to "Submitted"
    '                '    updateQuery = "UPDATE stuRegistration SET RegistrationApproved = '1', ApplicationStatus = 'Approved', AdmissionFeeStatus = 'Submitted', Remark = @Remark WHERE RegistrationID = @RegistrationID"
    '                'End If
    '                If action = "Verify" AndAlso currentStatus = "Verified" Then
    '                    ' Update to "Approved" and set AdmissionFeeStatus to "Submitted"
    '                    Dim updateQuery As String = "UPDATE stuRegistration SET RegistrationApproved = '1', ApplicationStatus = 'Approved', AdmissionFeeStatus = 'Submitted', Remark = @Remark WHERE RegistrationID = @RegistrationID"

    '                    Using updateCmd As New SqlCommand(updateQuery, conn)
    '                        updateCmd.Parameters.AddWithValue("@Remark", remark) ' Assuming `remark` is defined elsewhere
    '                        updateCmd.Parameters.AddWithValue("@RegistrationID", registrationID)
    '                        updateCmd.ExecuteNonQuery()
    '                    End Using

    '                    ' Fetch required values from stuRegistration for the current RegistrationID
    '                    Dim query As String = "SELECT SessionID, ayid, CourseID, Sem, UserID, batchid, SeatID, registrationfee, admissionapproved FROM stuRegistration WHERE RegistrationID = @RegistrationID"
    '                    Dim sessionID As String = ""
    '                    Dim ayid As String = ""
    '                    Dim courseID As String = ""
    '                    Dim sem As String = ""
    '                    Dim userID As String = ""
    '                    Dim batchID As String = ""
    '                    Dim seatID As String = ""
    '                    Dim registrationFee As String = ""
    '                    Dim admissionApproved As String = ""

    '                    Using fetchCmd As New SqlCommand(query, conn)
    '                        fetchCmd.Parameters.AddWithValue("@RegistrationID", registrationID)
    '                        Using reader As SqlDataReader = fetchCmd.ExecuteReader()
    '                            If reader.Read() Then
    '                                sessionID = Convert.ToString(reader("SessionID"))
    '                                ayid = Convert.ToString(reader("ayid"))
    '                                courseID = Convert.ToString(reader("CourseID"))
    '                                sem = Convert.ToString(reader("Sem"))
    '                                userID = Convert.ToString(reader("UserID"))
    '                                batchID = Convert.ToString(reader("batchid"))
    '                                seatID = Convert.ToString(reader("SeatID"))
    '                                registrationFee = Convert.ToString(reader("registrationfee"))
    '                                admissionApproved = Convert.ToString(reader("admissionapproved"))
    '                            End If
    '                        End Using
    '                    End Using

    '                    ' Call the stored procedure to process admission
    '                    Using spCmd As New SqlCommand("GetAdmisnno", conn)
    '                        spCmd.CommandType = CommandType.StoredProcedure

    '                        spCmd.Parameters.AddWithValue("@Sessionid", sessionID)
    '                        spCmd.Parameters.AddWithValue("@ayid", ayid)
    '                        spCmd.Parameters.AddWithValue("@Regisid", registrationID)
    '                        spCmd.Parameters.AddWithValue("@Courseid", courseID)
    '                        spCmd.Parameters.AddWithValue("@Sem", sem)
    '                        spCmd.Parameters.AddWithValue("@Userid", "1197") ' Assuming a default UserID or replace with actual logic
    '                        spCmd.Parameters.AddWithValue("@batchid", batchID)
    '                        spCmd.Parameters.AddWithValue("@SeatID", seatID)
    '                        spCmd.Parameters.AddWithValue("@registrationfee", registrationFee)
    '                        spCmd.Parameters.AddWithValue("@admissionapproved", admissionApproved)

    '                        spCmd.ExecuteNonQuery()
    '                    End Using
    '                End If

    '                ' Update the record
    '                Using cmd As New SqlCommand(updateQuery, conn)
    '                    ' Set parameters based on the selected action
    '                    If action <> "Verify" OrElse currentStatus <> "Verified" Then
    '                        Select Case action
    '                            Case "Verify"
    '                                cmd.Parameters.AddWithValue("@RegistrationApproved", "1")
    '                                cmd.Parameters.AddWithValue("@ApplicationStatus", "Verified")
    '                                cmd.Parameters.AddWithValue("@AdmissionFeeStatus", "")
    '                            Case "Reject"
    '                                cmd.Parameters.AddWithValue("@RegistrationApproved", "1")
    '                                cmd.Parameters.AddWithValue("@ApplicationStatus", "Rejected")
    '                                cmd.Parameters.AddWithValue("@AdmissionFeeStatus", "")
    '                            Case "Update"
    '                                cmd.Parameters.AddWithValue("@RegistrationApproved", "1")
    '                                cmd.Parameters.AddWithValue("@ApplicationStatus", "Pending")
    '                                cmd.Parameters.AddWithValue("@AdmissionFeeStatus", "")
    '                            Case Else
    '                                cmd.Parameters.AddWithValue("@RegistrationApproved", DBNull.Value)
    '                                cmd.Parameters.AddWithValue("@ApplicationStatus", DBNull.Value)
    '                                cmd.Parameters.AddWithValue("@AdmissionFeeStatus", "")
    '                        End Select
    '                    End If

    '                    ' Add the remark parameter (or DBNull if empty)
    '                    cmd.Parameters.AddWithValue("@Remark", If(String.IsNullOrEmpty(remark), DBNull.Value, remark))
    '                    ' Add the RegistrationID parameter
    '                    cmd.Parameters.AddWithValue("@RegistrationID", registrationID)

    '                    ' Execute the update command
    '                    cmd.ExecuteNonQuery()
    '                    rowsUpdated += 1 ' Increment the counter for updated rows
    '                End Using
    '            End If
    '        Next

    '        ' Provide user feedback
    '        If noSelection Then
    '            ' No rows were selected
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('Nothing Selected, Please select something.');", True)
    '        ElseIf rowsUpdated > 0 Then
    '            ' Rows updated successfully
    '            'ClientScript.RegisterStartupScript(Me.GetType(), "Success", $"alert('{rowsUpdated} record(s) updated successfully.');", True)
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('Record(s) updated successfully.');", True)
    '        Else
    '            ' No updates occurred
    '            'ClientScript.RegisterStartupScript(Me.GetType(), "NoUpdates", "alert('No records were updated.');", True)
    '            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "Alert", "showCustomAlert('No records were updated.');", True)
    '        End If

    '        ' Clear remarks and rebind the grid
    '        remarks.Text = ""
    '        BindGrid()
    '    End Using
    'End Sub





    Protected Sub ddlCourses_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlCourses.SelectedIndexChanged
        Dim selectedCoursePrefix As String = ddlCourses.SelectedValue

        ' Get the current status from the query string
        Dim status As String = Request.QueryString("status")
        If String.IsNullOrEmpty(status) Then status = "All"

        ' Check if the "All Courses" option is selected
        If selectedCoursePrefix = "All" Then
            ' Redirect to show all courses and current status
            Response.Redirect(String.Format("AdmissionApprove.aspx?courseid=All&status={0}", status))
        Else
            ' Redirect with the selected course and status
            Response.Redirect(String.Format("AdmissionApprove.aspx?courseid={0}&status={1}", selectedCoursePrefix, status))
        End If
    End Sub
End Class
