﻿Public Class Form2
    Private Sub btnback_Click(sender As Object, e As EventArgs) Handles btnback.Click

        Form1.Show()
        Me.Hide()
    End Sub

    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles Me.Load
        DbConnection()
        LoadUserType()
        With dgvrecord
            .AllowUserToDeleteRows = False
            .AllowUserToAddRows = False
            .AllowUserToResizeColumns = False
            .AllowUserToResizeRows = False
            .RowsDefaultCellStyle.BackColor = Color.SkyBlue
            .AlternatingRowsDefaultCellStyle.BackColor = Color.Blue
        End With
    End Sub

    Private Sub btnfind_ChangeUICues(sender As Object, e As UICuesEventArgs) Handles btnfind.ChangeUICues
        dgvrecord.DataBindings.Clear()
        dgvrecord.DataSource = Nothing
        If cbotype.SelectedIndex = 0 Then
            FindRecord(txtdata.Text, "Adviser")
        ElseIf cbotype.SelectedIndex = 1 Then
            FindRecord(txtdata.Text, "enlistment officer")
        ElseIf cbotype.SelectedIndex = 2 Then
            FindRecord(txtdata.Text, "finance officer")


        End If
    End Sub

    Private Sub dgvrecord_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvrecord.CellFormatting
        If dgvrecord.Columns(e.ColumnIndex).Index = 2 Then
            If e.Value <> " " Then
                e.Value = "*******"
                e.FormattingApplied = True
            End If
        End If
    End Sub
End Class