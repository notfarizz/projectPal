Imports System.Data.Odbc
Public Class FormMasterBarang
    Sub KondisiAwal()
        TextBox1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        ComboBox1.Text = ""
        TextBox1.Enabled = False
        TextBox2.Enabled = False
        TextBox3.Enabled = False
        TextBox4.Enabled = False
        ComboBox1.Enabled = False
        Button1.Enabled = True
        Button2.Enabled = True
        Button3.Enabled = True
        Button1.Text = "Input"
        Button2.Text = "Edit"
        Button3.Text = "Hapus"
        Button4.Text = "Tutup"
        Call Koneksi()
        Da = New OdbcDataAdapter("Select * From tbl_barang", Conn)
        Ds = New DataSet
        Da.Fill(Ds, "tbl_barang")
        DataGridView1.DataSource = Ds.Tables("tbl_barang")
        DataGridView1.ReadOnly = True
    End Sub

    Sub SiapIsi()
        TextBox2.Enabled = True
        TextBox3.Enabled = True
        TextBox4.Enabled = True
        ComboBox1.Enabled = True
        Call MunculSatuan()
    End Sub

    Sub NomorOtomatis()
        Call Koneksi()
        Cmd = New OdbcCommand("Select * from tbl_barang where kodebarang in (select max(kodebarang) from tbl_barang)", Conn)
        Dim UrutanKode As String
        Dim Hitung As Long
        Rd = Cmd.ExecuteReader
        Rd.Read()
        If Not Rd.HasRows Then
            UrutanKode = "BRG" + "001"
        Else
            Hitung = Microsoft.VisualBasic.Right(Rd.GetString(0), 3) + 1
            UrutanKode = "BRG" + Microsoft.VisualBasic.Right("000" & Hitung, 3)
        End If
        TextBox1.Text = UrutanKode
    End Sub



    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Button1.Text = "Input" Then
            TextBox1.Enabled = False
            Button1.Text = "Simpan"
            Button2.Enabled = False
            Button3.Enabled = False
            Button4.Text = "Batal"
            Call SiapIsi()
            Call NomorOtomatis()
        Else
            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Then
                MsgBox("Silahkan isi semua Field")
            Else
                Call Koneksi()
                ' Pengecekan apakah Nama Barang sudah ada di database
                Dim CekNamaBarang As String = "SELECT COUNT(*) FROM tbl_barang WHERE namabarang = '" & TextBox2.Text & "'"
                Cmd = New OdbcCommand(CekNamaBarang, Conn)
                Dim JumlahNamaBarang As Integer = Cmd.ExecuteScalar()

                If JumlahNamaBarang > 0 Then
                    MsgBox("Nama barang sudah terdaftar di database!")
                Else
                    ' Proses input data baru jika nama barang belum ada
                    Dim InputData As String = "INSERT INTO tbl_barang VALUES('" & TextBox1.Text & "','" & TextBox2.Text & "','" & TextBox3.Text & "','" & TextBox4.Text & "','" & ComboBox1.Text & "')"
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
            Cmd = New OdbcCommand("Select * From tbl_barang where kodebarang='" &
            TextBox1.Text & "'", Conn)
            Rd = Cmd.ExecuteReader
            Rd.Read()
            If Not Rd.HasRows Then
                MsgBox("Kode barang tidak Ada")
            Else
                TextBox1.Text = Rd.Item("Kodebarang")
                TextBox2.Text = Rd.Item("Namabarang")
                TextBox3.Text = Rd.Item("hargabarang")
                TextBox4.Text = Rd.Item("jumlahbarang")
                ComboBox1.Text = Rd.Item("satuanbarang")
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
            TextBox2.Enabled = True
            Call SiapIsi()
        Else
            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Then
                MsgBox("Silahkan isi semua Field")
            Else
                Call Koneksi()
                ' Pengecekan kesesuaian kode barang dan nama barang di database
                Dim CekData As String = "SELECT COUNT(*) FROM tbl_barang WHERE kodebarang = '" & TextBox1.Text & "' AND namabarang = '" & TextBox2.Text & "'"
                Cmd = New OdbcCommand(CekData, Conn)
                Dim DataSesuai As Integer = Cmd.ExecuteScalar()

                If DataSesuai = 0 Then
                    MsgBox("Kode barang dan nama barang tidak sesuai!")
                Else
                    ' Proses hapus data jika kode dan nama barang sesuai
                    Dim HapusData As String = "DELETE FROM tbl_barang WHERE kodebarang='" & TextBox1.Text & "' AND namabarang='" & TextBox2.Text & "'"
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
                Call Koneksi()
                ' Pengecekan apakah Nama Barang sudah ada pada barang lain di database
                Dim CekNamaBarang As String = "SELECT COUNT(*) FROM tbl_barang WHERE namabarang = '" & TextBox2.Text & "' AND kodebarang <> '" & TextBox1.Text & "'"
                Cmd = New OdbcCommand(CekNamaBarang, Conn)
                Dim JumlahNamaBarang As Integer = Cmd.ExecuteScalar()

                If JumlahNamaBarang > 0 Then
                    MsgBox("Nama barang sudah terdaftar!")
                Else
                    ' Proses update data jika nama barang tidak duplikat
                    Dim UpdateData As String = "UPDATE tbl_barang SET namabarang='" & TextBox2.Text & "', hargabarang='" & TextBox3.Text & "', jumlahbarang='" & TextBox4.Text & "', satuanbarang='" & ComboBox1.Text & "' WHERE kodebarang='" & TextBox1.Text & "'"
                    Cmd = New OdbcCommand(UpdateData, Conn)
                    Cmd.ExecuteNonQuery()
                    MsgBox("Update Data Berhasil")
                    Call KondisiAwal()
                End If
            End If
        End If
    End Sub


    Private Sub FormMasterBarang_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call KondisiAwal()
    End Sub

    Private Sub TextBox3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox3.KeyPress
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack) Then
            e.Handled = True
        End If
    End Sub

    Private Sub TextBox4_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox4.KeyPress
        If Not ((e.KeyChar >= "0" And e.KeyChar <= "9") Or e.KeyChar = vbBack) Then
            e.Handled = True
        End If
    End Sub

    Sub MunculSatuan()
        Call Koneksi()
        Cmd = New OdbcCommand("select distinct satuanbarang from tbl_barang", Conn)
        Rd = Cmd.ExecuteReader
        ComboBox1.Items.Clear()
        Do While Rd.Read
            ComboBox1.Items.Add(Rd.Item("satuanbarang"))
        Loop
    End Sub
End Class