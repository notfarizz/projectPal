Imports System.Data.Odbc
Public Class FormMasterPelanggan
    Sub KondisiAwal()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""

        TextBox4.Text = ""
        TextBox1.Enabled = False
        TextBox2.Enabled = False
        TextBox3.Enabled = False
        TextBox4.Enabled = False
        Button1.Enabled = True
        Button2.Enabled = True
        Button3.Enabled = True
        Button1.Text = "Input"
        Button2.Text = "Edit"
        Button3.Text = "Hapus"
        Button4.Text = "Tutup"
        Call Koneksi()
        Da = New OdbcDataAdapter("Select * From tbl_pelanggan", Conn)
        Ds = New DataSet
        Da.Fill(Ds, "tbl_Pelanggan")
        DataGridView1.DataSource = Ds.Tables("tbl_pelanggan")
        DataGridView1.ReadOnly = True
    End Sub

    Sub SiapIsi()
        TextBox2.Enabled = True
        TextBox3.Enabled = True
        TextBox4.Enabled = True
    End Sub
    Sub NomorOtomatis()
        Call Koneksi()
        Cmd = New OdbcCommand("Select * from tbl_pelanggan where kodepelanggan in (select max(kodepelanggan) from tbl_pelanggan)", Conn)
        Dim UrutanKode As String
        Dim Hitung As Long
        Rd = Cmd.ExecuteReader
        Rd.Read()
        If Not Rd.HasRows Then
            UrutanKode = "PLG" + "001"
        Else
            Hitung = Microsoft.VisualBasic.Right(Rd.GetString(0), 3) + 1
            UrutanKode = "PLG" + Microsoft.VisualBasic.Right("000" & Hitung, 3)
        End If
        TextBox1.Text = UrutanKode
    End Sub

    Private Sub FormMasterPelanggan_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call KondisiAwal()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Button1.Text = "Input" Then
            Button1.Text = "Simpan"
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Text = "Batal"
            TextBox1.Enabled = False
            Call SiapIsi()
            Call NomorOtomatis()
        Else
            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Then
                MsgBox("Silahkan isi semua Field")
            Else
                ' Pengecekan apakah nomor telepon sudah ada
                Call Koneksi()
                Dim CekTelepon As String = "SELECT COUNT(*) FROM tbl_pelanggan WHERE telppelanggan = '" & TextBox4.Text & "'"
                Cmd = New OdbcCommand(CekTelepon, Conn)
                Dim JumlahTelepon As Integer = Cmd.ExecuteScalar()

                If JumlahTelepon > 0 Then
                    MsgBox("Nomor telepon sudah terdaftar!")
                Else
                    ' Proses menyimpan data baru jika nomor telepon belum ada
                    Dim InputData As String = "INSERT INTO tbl_pelanggan VALUES('" & TextBox1.Text & "','" & TextBox2.Text & "','" & TextBox3.Text & "','" & TextBox4.Text & "')"
                    Cmd = New OdbcCommand(InputData, Conn)
                    Cmd.ExecuteNonQuery()
                    MsgBox("Input Data Berhasil")
                    Call KondisiAwal()
                End If
            End If
        End If
    End Sub



    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        If e.KeyChar = Chr(13) Then
            Call Koneksi()
            Cmd = New OdbcCommand("Select * From tbl_pelanggan where kodepelanggan='" & TextBox1.Text & "'", Conn)
            Rd = Cmd.ExecuteReader
            Rd.Read()
            If Not Rd.HasRows Then
                MsgBox("Kode Pelanggan tidak Ada")
            Else
                TextBox1.Text = Rd.Item("KodePelanggan")
                TextBox2.Text = Rd.Item("NamaPelanggan")
                TextBox3.Text = Rd.Item("AlamatPelanggan")
                TextBox4.Text = Rd.Item("telppelanggan")
            End If
        End If
    End Sub


    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If Button4.Text = "Tutup" Then
            Me.Close()
        Else
            Call KondisiAwal()
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If Button3.Text = "Hapus" Then
            Button3.Text = "Delete"
            Button1.Enabled = False
            Button2.Enabled = False
            Button4.Text = "Batal"
            TextBox1.Enabled = True
            Call SiapIsi()
        Else
            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Then
                MsgBox("Silahkan isi semua Field")
            Else
                ' Pengecekan data berdasarkan kode pelanggan dan nomor telepon
                Call Koneksi()
                Dim CekData As String = "SELECT COUNT(*) FROM tbl_pelanggan WHERE kodepelanggan='" & TextBox1.Text & "' AND telppelanggan='" & TextBox4.Text & "'"
                Cmd = New OdbcCommand(CekData, Conn)
                Dim JumlahData As Integer = Cmd.ExecuteScalar()

                If JumlahData = 0 Then
                    MsgBox("Data tidak ditemukan atau kode pelanggan dan nomor telepon tidak sesuai!")
                Else
                    ' Menghapus data jika kode pelanggan dan nomor telepon sesuai
                    Dim HapusData As String = "DELETE FROM tbl_pelanggan WHERE kodepelanggan='" & TextBox1.Text & "' AND telppelanggan='" & TextBox4.Text & "'"
                    Cmd = New OdbcCommand(HapusData, Conn)
                    Cmd.ExecuteNonQuery()
                    MsgBox("Hapus Data Berhasil")
                    Call KondisiAwal()
                End If
            End If
        End If
    End Sub


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If Button2.Text = "Edit" Then
            Button2.Text = "Simpan"
            Button1.Enabled = False
            Button3.Enabled = False
            Button4.Text = "Batal"
            TextBox1.Enabled = True
            Call SiapIsi()
        Else
            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Then
                MsgBox("Silahkan isi semua Field")
            Else
                ' Pengecekan nomor telepon apakah sudah ada di database kecuali untuk pelanggan yang sedang diedit
                Call Koneksi()
                Dim CekTelepon As String = "SELECT COUNT(*) FROM tbl_pelanggan WHERE telppelanggan = '" & TextBox4.Text & "' AND kodepelanggan <> '" & TextBox1.Text & "'"
                Cmd = New OdbcCommand(CekTelepon, Conn)
                Dim JumlahTelepon As Integer = Cmd.ExecuteScalar()

                If JumlahTelepon > 0 Then
                    MsgBox("Nomor telepon sudah terdaftar untuk pelanggan lain!")
                Else
                    ' Proses update data jika nomor telepon tidak duplikat
                    Dim UpdateData As String = "UPDATE tbl_pelanggan SET namapelanggan='" & TextBox2.Text & "', alamatpelanggan='" & TextBox3.Text & "', telppelanggan='" & TextBox4.Text & "' WHERE kodepelanggan='" & TextBox1.Text & "'"
                    Cmd = New OdbcCommand(UpdateData, Conn)
                    Cmd.ExecuteNonQuery()
                    MsgBox("Update Data Berhasil")
                    Call KondisiAwal()
                End If
            End If
        End If
    End Sub

End Class