﻿Imports System.Data.Odbc
Public Class FormLapJual

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        AxCrystalReport1.SelectionFormula = "totext({tbl_Jual.tglJual})='" & DateTimePicker1.Value & "'"
        AxCrystalReport1.ReportFileName = "LaporanHarian.rpt"
        AxCrystalReport1.WindowState = Crystal.WindowStateConstants.crptMaximized
        AxCrystalReport1.RetrieveDataFiles()
        AxCrystalReport1.Action = 1
    End Sub

    Private Sub FormLapJual_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ComboBox1.Items.Clear()
        ComboBox1.Items.Add("01 - JANUARI")
        ComboBox1.Items.Add("02 - FEBRUARI")
        ComboBox1.Items.Add("03 - MARET")
        ComboBox1.Items.Add("04 - APRIL")
        ComboBox1.Items.Add("05 - MEI")
        ComboBox1.Items.Add("06 - JUNI")
        ComboBox1.Items.Add("07 - JULI")
        ComboBox1.Items.Add("08 - AGUSTUS")
        ComboBox1.Items.Add("09 - SEPTEMBER")
        ComboBox1.Items.Add("10 - OKTOBER")
        ComboBox1.Items.Add("11 - NOVEMBER")
        ComboBox1.Items.Add("12 - DESEMBER")
        ComboBox2.Items.Clear()
        ComboBox2.Text = Date.Now.Year
        For i As Integer = 0 To 5
            ComboBox2.Items.Add(Date.Now.Year - i)
        Next

        Label7.Text = Format(DateTimePicker2.Value, "yyyy, MM, dd")
        Label8.Text = Format(DateTimePicker3.Value, "yyyy, MM, dd")
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If ComboBox1.Text = "" Or ComboBox2.Text = "" Then
            MsgBox("Silahkan Isi Bulan dan Tahun terlebih dahulu")
        Else
            AxCrystalReport1.SelectionFormula = "Month({tbl_Jual.TglJual})=" &
            Val(ComboBox1.Text) & " and year({tbl_Jual.TglJual})=" & Val(ComboBox2.Text)
            AxCrystalReport1.ReportFileName = "LaporanBulanan.rpt"
            AxCrystalReport1.WindowState = Crystal.WindowStateConstants.crptMaximized
            AxCrystalReport1.RetrieveDataFiles()
            AxCrystalReport1.Action = 1
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        AxCrystalReport1.SelectionFormula = "{tbl_Jual.TglJual} in date (" &
Label7.Text & ") to date (" & Label8.Text & ")"
        AxCrystalReport1.ReportFileName = "LaporanMingguan.rpt"
        AxCrystalReport1.WindowState = Crystal.WindowStateConstants.crptMaximized
        AxCrystalReport1.RetrieveDataFiles()
        AxCrystalReport1.Action = 1
    End Sub

    Private Sub DateTimePicker2_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker2.ValueChanged
        Label7.Text = Format(DateTimePicker2.Value, "yyyy, MM, dd")
    End Sub

    Private Sub DateTimePicker3_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker3.ValueChanged
        Label8.Text = Format(DateTimePicker3.Value, "yyyy, MM, dd")
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim cmd
        Dim hasil As Integer
        cmd = New OdbcCommand("select count(*) from tbl_jual where NoJual = ?", Conn)
        cmd.Parameters.AddWithValue("@nota", TextBox1.Text)
        hasil = CInt(cmd.ExecuteScalar())
        If TextBox1.Text = "" Then
            MessageBox.Show("Nomor Nota tidak boleh kosong!!!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        ElseIf hasil = 0 Then
            MessageBox.Show("Nomor Nota tidak ada!!!", "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Else
            AxCrystalReport1.SelectionFormula = "totext({tbl_Jual.NoJual})='" & TextBox1.Text & "'"
            AxCrystalReport1.ReportFileName = "notajual.rpt"
            AxCrystalReport1.WindowState =
            Crystal.WindowStateConstants.crptMaximized
            AxCrystalReport1.RetrieveDataFiles()
            AxCrystalReport1.Action = 1
        End If

    End Sub
End Class