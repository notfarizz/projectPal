Imports System.Data.Odbc
Public Class FormMasterSupplier
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
        Da = New OdbcDataAdapter("Select * From tbl_supplier", Conn)
        Ds = New DataSet
        Da.Fill(Ds, "tbl_supplier")
        DataGridView1.DataSource = Ds.Tables("tbl_supplier")
        DataGridView1.ReadOnly = True
    End Sub

    Sub SiapIsi()
        TextBox2.Enabled = True
        TextBox3.Enabled = True
        TextBox4.Enabled = True
    End Sub
    Sub NomorOtomatis()
        Call Koneksi()
        Cmd = New OdbcCommand("Select * from tbl_supplier where kodesupplier in (select max(kodesupplier) from tbl_supplier)", Conn)
        Dim UrutanKode As String
        Dim Hitung As Long
        Rd = Cmd.ExecuteReader
        Rd.Read()
        If Not Rd.HasRows Then
            UrutanKode = "SUP" + "001"
        Else
            Hitung = Microsoft.VisualBasic.Right(Rd.GetString(0), 3) + 1
            UrutanKode = "SUP" + Microsoft.VisualBasic.Right("000" & Hitung, 3)
        End If
        TextBox1.Text = UrutanKode
    End Sub

    Private Sub FormMasterSupplier_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
                Dim CekTelepon As String = "SELECT COUNT(*) FROM tbl_supplier WHERE telpsupplier = '" & TextBox4.Text & "'"
                Cmd = New OdbcCommand(CekTelepon, Conn)
                Dim JumlahTelepon As Integer = Cmd.ExecuteScalar()

                If JumlahTelepon > 0 Then
                    MsgBox("Nomor telepon sudah terdaftar untuk supplier lain!")
                Else
                    ' Proses menyimpan data baru jika nomor telepon belum ada
                    Dim InputData As String = "INSERT INTO tbl_supplier VALUES('" & TextBox1.Text & "','" & TextBox2.Text & "','" & TextBox3.Text & "','" & TextBox4.Text & "')"
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
            Cmd = New OdbcCommand("Select * From tbl_supplier where kodesupplier='" & TextBox1.Text & "'", Conn)
            Rd = Cmd.ExecuteReader
            Rd.Read()
            If Not Rd.HasRows Then
                MsgBox("Kode Supplier tidak Ada")
            Else
                TextBox1.Text = Rd.Item("KodeSupplier")
                TextBox2.Text = Rd.Item("NamaSupplier")
                TextBox3.Text = Rd.Item("AlamatSupplier")
                TextBox4.Text = Rd.Item("telpsupplier")
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
                ' Pengecekan apakah kode dan telepon sesuai
                Call Koneksi()
                Dim CekData As String = "SELECT COUNT(*) FROM tbl_supplier WHERE kodesupplier = '" & TextBox1.Text & "' AND telpsupplier = '" & TextBox4.Text & "'"
                Cmd = New OdbcCommand(CekData, Conn)
                Dim JumlahData As Integer = Cmd.ExecuteScalar()

                If JumlahData = 0 Then
                    MsgBox("Kode Supplier dan Nomor Telepon tidak cocok atau tidak ada di database.")
                Else
                    ' Proses hapus data jika kode dan telepon sesuai
                    Dim HapusData As String = "DELETE FROM tbl_supplier WHERE kodesupplier = '" & TextBox1.Text & "' AND telpsupplier = '" & TextBox4.Text & "'"
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
                ' Pengecekan nomor telepon apakah sudah ada di database kecuali untuk supplier yang sedang diedit
                Call Koneksi()
                Dim CekTelepon As String = "SELECT COUNT(*) FROM tbl_supplier WHERE telpsupplier = '" & TextBox4.Text & "' AND kodesupplier <> '" & TextBox1.Text & "'"
                Cmd = New OdbcCommand(CekTelepon, Conn)
                Dim JumlahTelepon As Integer = Cmd.ExecuteScalar()

                If JumlahTelepon > 0 Then
                    MsgBox("Nomor telepon sudah terdaftar untuk supplier lain!")
                Else
                    ' Proses update data jika nomor telepon tidak duplikat
                    Dim UpdateData As String = "UPDATE tbl_supplier SET namasupplier='" & TextBox2.Text & "', alamatsupplier='" & TextBox3.Text & "', telpsupplier='" & TextBox4.Text & "' WHERE kodesupplier='" & TextBox1.Text & "'"
                    Cmd = New OdbcCommand(UpdateData, Conn)
                    Cmd.ExecuteNonQuery()
                    MsgBox("Update Data Berhasil")
                    Call KondisiAwal()
                End If
            End If
        End If
    End Sub

End Class