
Partial Class FTB
    Inherits System.Web.UI.Page
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Retrieve the rich text content
        Dim richTextContent As String = Request.Form("txtRichText")

        ' Display the submitted rich text content in the Label
        lblOutput.Text = "You entered:<br />" & richTextContent
    End Sub
End Class
