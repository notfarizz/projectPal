Imports System.Data.Odbc

Public Class FormTransJual
    Dim TglMySql As String

    Sub KondisiAwal()
        LBLNamaPlg.Text = ""
        LBLAlamat.Text = ""
        txtTelepon.Text = ""
        LBLTanggal.Text = Today.ToString("dd-MM-yyyy")
        LBLAdmin.Text = FormMenuUtama.STLabel4.Text
        LBLKembali.Text = ""
        TextBox1.Text = ""
        LBLNamaBarang.Text = ""
        LBLHargaBarang.Text = ""
        TextBox1.Text = ""
        TextBox1.Enabled = False
        LBLItem.Text = ""
        Call MunculKodePelanggan()
        Call NomorOtomatis()
        Call BuatKolom()
        Label9.Text = "0"
        TextBox2.Text = ""
    End Sub

    Sub MunculKodePelanggan()
        Call Koneksi()
        Cmd = New OdbcCommand("SELECT * FROM tbl_Pelanggan", Conn)
        Rd = Cmd.ExecuteReader
        ComboBox1.Items.Clear()
        Do While Rd.Read
            ComboBox1.Items.Add(Rd.Item(0))
        Loop
    End Sub

    Private Sub FormTransJual_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Call KondisiAwal()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        LBLJam.Text = TimeOfDay.ToString("HH:mm:ss")
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox1.SelectedIndexChanged
        Call Koneksi()
        Cmd = New OdbcCommand("SELECT * FROM tbl_Pelanggan WHERE kodepelanggan ='" & ComboBox1.Text & "'", Conn)
        Rd = Cmd.ExecuteReader
        If Rd.Read Then
            LBLNamaPlg.Text = Rd!NamaPelanggan
            LBLAlamat.Text = Rd!AlamatPelanggan
            txtTelepon.Text = Rd!TelpPelanggan
        End If
    End Sub

    ' Event ini akan menggantikan txtTelepon_TextChanged
    Private Sub txtTelepon_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtTelepon.KeyPress
        If e.KeyChar = Chr(13) Then
            Call Koneksi()
            Cmd = New OdbcCommand("SELECT * FROM tbl_Pelanggan WHERE TelpPelanggan = '" & txtTelepon.Text & "'", Conn)
            Rd = Cmd.ExecuteReader
            If Rd.Read Then
                LBLNamaPlg.Text = Rd!NamaPelanggan
                LBLAlamat.Text = Rd!AlamatPelanggan
                ComboBox1.Text = Rd!kodepelanggan
            Else
                LBLNamaPlg.Text = ""
                LBLAlamat.Text = ""
                ComboBox1.Text = ""
                MsgBox("Nomor telepon tidak ditemukan.")
            End If
        End If
    End Sub

    Sub NomorOtomatis()
        Call Koneksi()
        Cmd = New OdbcCommand("SELECT * FROM tbl_jual WHERE nojual IN (SELECT MAX(nojual) FROM tbl_jual)", Conn)
        Dim UrutanKode As String
        Dim Hitung As Long
        Rd = Cmd.ExecuteReader
        If Rd.Read() Then
            Hitung = Microsoft.VisualBasic.Right(Rd.GetString(0), 9) + 1
            UrutanKode = "J" + Format(Now, "yyMMdd") + Microsoft.VisualBasic.Right("000" & Hitung, 3)
        Else
            UrutanKode = "J" + Format(Now, "yyMMdd") + "001"
        End If
        LBLNoJual.Text = UrutanKode
    End Sub

    Sub BuatKolom()
        DataGridView1.Columns.Clear()
        DataGridView1.Columns.Add("Kode", "Kode")
        DataGridView1.Columns.Add("Nama", "Nama Barang")
        DataGridView1.Columns.Add("Harga", "Harga")
        DataGridView1.Columns.Add("Jumlah", "Jumlah")
        DataGridView1.Columns.Add("Subtotal", "Subtotal")
    End Sub

    Private Sub TextBox2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox2.KeyPress
        If e.KeyChar = Chr(13) Then
            Call Koneksi()
            Cmd = New OdbcCommand("SELECT * FROM tbl_barang WHERE kodebarang='" & TextBox2.Text & "'", Conn)
            Rd = Cmd.ExecuteReader
            If Rd.Read() Then
                TextBox2.Text = Rd("Kodebarang").ToString
                LBLNamaBarang.Text = Rd("Namabarang").ToString
                LBLHargaBarang.Text = Rd("hargabarang").ToString
                LBLJumlah.Text = Rd("jumlahbarang").ToString
                TextBox1.Enabled = True
                TextBox1.Focus()
            Else
                MsgBox("Kode barang tidak ada.")
            End If
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If LBLNamaBarang.Text = "" Or TextBox1.Text = "" Then
            MsgBox("Silahkan masukan kode barang dan tekan ENTER!")
        Else
            If Val(LBLJumlah.Text) < Val(TextBox1.Text) Then
                MsgBox("Kuantitas pembelian melebihi stok")
            Else
                DataGridView1.Rows.Add(New String() {TextBox2.Text, LBLNamaBarang.Text, LBLHargaBarang.Text, TextBox1.Text, Val(LBLHargaBarang.Text) * Val(TextBox1.Text)})
                Call RumusSubtotal()
                TextBox2.Text = ""
                LBLNamaBarang.Text = ""
                LBLHargaBarang.Text = ""
                TextBox1.Text = ""
                TextBox1.Enabled = False
                Call RumusCariItem()
            End If
        End If
    End Sub

    Sub RumusSubtotal()
        Dim hitung As Integer = 0
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            hitung += DataGridView1.Rows(i).Cells(4).Value
        Next
        Label9.Text = hitung.ToString()
    End Sub

    Sub RumusCariItem()
        Dim HitungItem As Integer = 0
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            HitungItem += DataGridView1.Rows(i).Cells(3).Value
        Next
        LBLItem.Text = HitungItem.ToString()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If LBLKembali.Text = "" Or LBLNamaPlg.Text = "" Or Label9.Text = "" Then
            MsgBox("Transaksi tidak ada, silahkan lakukan transaksi terlebih dahulu.")
        Else
            TglMySql = Format(Today, "yyyy-MM-dd")
            Dim SimpanJual As String = "INSERT INTO tbl_jual VALUES ('" & LBLNoJual.Text & "', '" & TglMySql & "', '" & LBLJam.Text & "', '" & LBLItem.Text & "', '" & Label9.Text & "', '" & TextBox3.Text & "', '" & LBLKembali.Text & "', '" & ComboBox1.Text & "', '" & FormMenuUtama.STLabel2.Text & "') "
            Cmd = New OdbcCommand(SimpanJual, Conn)
            Cmd.ExecuteNonQuery()

            For Baris As Integer = 0 To DataGridView1.Rows.Count - 2
                Dim SimpanDetail As String = "INSERT INTO tbl_detailjual VALUES('" & LBLNoJual.Text & "', '" & DataGridView1.Rows(Baris).Cells(0).Value & "', '" & DataGridView1.Rows(Baris).Cells(1).Value & "', '" & DataGridView1.Rows(Baris).Cells(2).Value & "', '" & DataGridView1.Rows(Baris).Cells(3).Value & "','" & DataGridView1.Rows(Baris).Cells(4).Value & "')"
                Cmd = New OdbcCommand(SimpanDetail, Conn)
                Cmd.ExecuteNonQuery()

                Cmd = New OdbcCommand("SELECT * FROM tbl_barang WHERE kodebarang='" & DataGridView1.Rows(Baris).Cells(0).Value & "'", Conn)
                Rd = Cmd.ExecuteReader
                If Rd.Read() Then
                    Dim KurangiStok As String = "UPDATE tbl_barang SET JumlahBarang = '" & (Rd("JumlahBarang") - DataGridView1.Rows(Baris).Cells(3).Value) & "' WHERE KodeBarang='" & DataGridView1.Rows(Baris).Cells(0).Value & "'"
                    Cmd = New OdbcCommand(KurangiStok, Conn)
                    Cmd.ExecuteNonQuery()
                End If
            Next

            If MessageBox.Show("Apakah ingin cetak nota?", "", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
                AxCrystalReport1.SelectionFormula = "totext({tbl_jual.NoJual})='" & LBLNoJual.Text & "'"
                AxCrystalReport1.ReportFileName = "notajual.rpt"
                AxCrystalReport1.WindowState = Crystal.WindowStateConstants.crptMaximized
                AxCrystalReport1.RetrieveDataFiles()
                AxCrystalReport1.Action = 1
            End If
            Call KondisiAwal()
            MsgBox("Transaksi Telah Berhasil Disimpan.")
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Call KondisiAwal()
    End Sub

    Private Sub TextBox3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox3.KeyPress
        If e.KeyChar = Chr(13) Then
            If Val(TextBox3.Text) < Val(Label9.Text) Then
                MsgBox("Pembayaran kurang!")
            ElseIf Val(TextBox3.Text) = Val(Label9.Text) Then
                LBLKembali.Text = "0"
            Else
                LBLKembali.Text = (Val(TextBox3.Text) - Val(Label9.Text)).ToString()
                Button1.Focus()
            End If
        End If
    End Sub
End Class
