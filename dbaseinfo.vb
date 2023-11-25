﻿Imports MySql.Data.MySqlClient

Module dbaseinfo
    Dim conn As New MySqlConnection
    Dim strquery, query As String
    Dim reader As MySqlDataReader
    Dim dbname, server, username, password As String
    Dim cmd As New MySqlCommand
    Dim adapter As MySqlDataAdapter
    Dim dataTable As New DataTable
    Dim dataset As DataSet


    Public Sub DbConnection()
        dbname = "it2aoop"
        server = "127.0.0.1"
        username = "root"
        password = "password"
        If Not conn Is Nothing Then
            conn.Close()
            conn.ConnectionString = "server =" & server & ";" & "user id=" & username & ";password=" & password & ";database=" & dbname & ""
            Try
                conn.Open()
                MsgBox("Connected!")


            Catch ex As Exception
                MsgBox(ex.Message)

            End Try
        End If
    End Sub
    Public Sub InsertRec()
        query = "INSERT INTO tbluser(uname, pass) VALUES(@Param1, @Param2)"
        cmd = New MySqlCommand(query, conn)
        cmd.Parameters.AddWithValue("Param1", Form1.txtuname.Text)
        cmd.Parameters.AddWithValue("Param2", Form1.txtpass.Text)
        Try
            cmd.ExecuteNonQuery()
            MessageBox.Show("Record inserted successfully!")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            ClearData()

        End Try
    End Sub
    Public Sub ClearData()
        Form1.txtuname.Clear()
        Form1.txtpass.Clear()
    End Sub

    Public Sub SearchRec()
        query = "SELECT *FROM tbluser where userid = @Param1"
        cmd = New MySqlCommand(query, conn)
        cmd.Parameters.AddWithValue("@Param1", Form1.txtuid.Text)
        reader = cmd.ExecuteReader()
        'check if record exist
        Try
            If reader.Read() Then
                'display the record using textboxes
                Form1.txtusername.Text = reader("uname").ToString()
                Form1.txtpassword.Text = reader("pass").ToString()
                Form1.btnupdate.Enabled = True
                Form1.btndelete.Enabled = True
                Form1.txtusername.Enabled = True
                Form1.txtpassword.Enabled = True
            Else
                MsgBox("No Record Found")
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            reader.Close()
        End Try

    End Sub

    Public Sub LoadUserType()
        query = "select usertype from tbluser"
        Try
            cmd = New MySqlCommand(query, conn)
            reader = cmd.ExecuteReader()
            While reader.Read()
                Form2.cbotype.Items.Add(reader("usertpe").ToString)
            End While
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            conn.Close()





        End Try
    End Sub


    Public Sub FindRecord(SearchData As String, utype As String)
        Form2.dgvrecord.DataSource = Nothing
        query = "SELECT * FROM tbluser where userid = @uid or usame = @username and user type = @utype"
        adapter = New MySqlDataAdapter(query, conn)
        adapter.SelectCommand.Parameters.AddWithValue("@uid", SearchData)
        adapter.SelectCommand.Parameters.AddWithValue("@username", SearchData)
        adapter.SelectCommand.Parameters.AddWithValue("@utype", utype)
        Try
            adapter.Fill(dataTable)
            Form2.dgvrecord.DataSource = dataTable

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Form2.txtdata.Clear()
        End Try
    End Sub
    Public Sub updateRecord(uid As String, uname As String, pass As String)
        query = "Update user SET uname=@uname,pass=@pass where userid=@uid"
        Try
            Using cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@uname", uname)
                cmd.Parameters.AddWithValue("@pass", pass)
                cmd.Parameters.AddWithValue("@uid", uid)
                cmd.ExecuteNonQuery()

            End Using

        Catch ex As Exception
            MessageBox.Show("Error: " + ex.Message)
        Finally
            conn.Close()
            Form1.txtpassword.Clear()
            Form1.txtusername.Clear()
            Form1.txtuid.Clear()
            Form1.txtpassword.Enabled = False
            Form1.txtusername.Enabled = False
        End Try
    End Sub
    Public Sub DeleteRecord(uid As String)
        query = "Delete from tbluser where userid = @uid"
        Try
            Dim cmd As New MySqlCommand(query, conn)
            cmd.Parameters.AddWithValue("uid,", uid)
            cmd.ExecuteNonQuery()
            MessageBox.Show("Record Delete Succesfuly")

        Catch ex As Exception
            MessageBox.Show("Error: " + ex.Message)
        Finally
            conn.Close()
            Form1.txtpassword.Clear()
            Form1.txtusername.Clear()
            Form1.txtuid.Clear()
            Form1.txtpassword.Enabled = False
            Form1.txtusername.Enabled = False
        End Try
    End Sub
    Public Sub LoadRecord()
        Form3.dgvrecord.DataSource = Nothing
        Try
            query = ("SELECT * FROM tbluser")
            adapter = New MySqlDataAdapter(query, conn)
            dataset = New DataSet()
            adapter.Fill(dataset, "tbluser")
            Form3.dgvrecord.DataSource = dataset.Tables("tbluser")

            Dim updateButton As New DataGridViewButtonColumn()
            updateButton.Name = "UpdateButton"
            updateButton.Text = "Update"
            updateButton.UseColumnTextForButtonValue = True
            Form3.dgvrecord.DataSource = dataset.Tables

            Dim DeleteButton As New DataGridViewButtonColumn()
            DeleteButton.Name = "DeleteButton"
            DeleteButton.Text = "Delete"
            DeleteButton.UseColumnTextForButtonValue = True
            Form3.dgvrecord.Columns.Add(DeleteButton)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub RecordUpdate(rowIndex As Integer, uname As String, pass As String)
        If rowIndex >= 0 Then
            Dim selectedRow = Form3.dgvrecord.Rows(rowIndex)
            Dim id As Integer = CInt(selectedRow.Cells("userid").Value)
            query = "UPDATE tbluser SET uname = @uname, pass = @pass WHERE userid = @id"
            Try
                conn.Open()
                Dim cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@uname", uname)
                cmd.Parameters.AddWithValue("@pass", pass)
                cmd.Parameters.AddWithValue("@uid", id)
                cmd.ExecuteNonQuery()
                MessageBox.Show("Record updated sucessfully")
            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                conn.Close()

            End Try

        End If
    End Sub
    Public Sub RemoveRecord(rowIndex As Integer)
        If rowIndex >= 0 Then
            Dim selectedRow = Form3.dgvrecord.Rows(rowIndex)
            Dim id As Integer = CInt(selectedRow.Cells("userid").Value)
            query = "DELETE FROM tbluser WHERE userid = @id"
            Try
                conn.Open()
                Dim cmd As New MySqlCommand(query, conn)
                cmd.Parameters.AddWithValue("@uid", id)
                cmd.ExecuteNonQuery()
                MessageBox.Show("Record deleted sucessfully")
                LoadRecord()
            Catch ex As Exception
                MsgBox(ex.Message)
            Finally
                conn.Close()

            End Try

        End If
    End Sub
End Module
