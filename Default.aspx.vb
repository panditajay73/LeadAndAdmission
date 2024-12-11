Imports ASPSnippets.GoogleAPI
Imports System.Web.Script.Serialization
Partial Class _Default
    Inherits System.Web.UI.Page
    Public Class GoogleProfile
        Public Property Id() As String
        Public Property Name() As String
        Public Property Picture() As String
        Public Property Email() As String
        Public Property Mobile() As String
        Public Property Verified_Email() As String
    End Class
    Protected Sub Login(ByVal sender As Object, ByVal e As EventArgs)
        GoogleConnect.Authorize("profile", "email", "https://www.googleapis.com/auth/user.phonenumbers.read")

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        GoogleConnect.ClientId = "Enter Your ClientID"
        GoogleConnect.ClientSecret = "Enter Your Secret key"
        GoogleConnect.RedirectUri = Request.Url.AbsoluteUri.Split("?"c)(0)

        If Not Me.IsPostBack Then
            Dim code As String = Request.QueryString("code")
            If Not String.IsNullOrEmpty(code) Then
                Dim connect As GoogleConnect = New GoogleConnect()
                Dim json As String = connect.Fetch("me", code)
                Dim profile As GoogleProfile = New JavaScriptSerializer().Deserialize(Of GoogleProfile)(json)
                lblId.Text = profile.Id
                lblName.Text = profile.Name
                lblEmail.Text = profile.Email
                lblMobile.Text = profile.Mobile
                lblVerified.Text = profile.Verified_Email
                imgProfile.ImageUrl = profile.Picture
                pnlProfile.Visible = True
                btnLogin.Enabled = False
            End If
        End If
    End Sub
End Class
