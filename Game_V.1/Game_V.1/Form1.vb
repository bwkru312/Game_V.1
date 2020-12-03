Public Class Form1
    Dim r As New Random
    Dim score As Integer
    Sub randMove(p As PictureBox)
        Dim x As Integer
        Dim y As Integer
        x = r.Next(-10, 11)
        MoveTo(p, x, y)
    End Sub
    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.Up
                MoveTo(PictureBox3, 0, -8)
            Case Keys.Down
                MoveTo(PictureBox3, 0, 8)
            Case Keys.Left
                MoveTo(PictureBox3, -8, 0)
            Case Keys.Right
                MoveTo(PictureBox3, 8, 0)
            Case Keys.Space
                Bullet.Location = PictureBox3.Location
                Bullet.Visible = True
                Timer2.Enabled = True
            Case Keys.F
                TextBox1.Visible = True
                TextBox1.Text = "Thanks for putting an F in the chat. Press x to leave"
            Case Keys.Q
                TextBox1.Visible = True
                TextBox1.Text = "Why would you press Q? Press x to leave"
            Case Keys.X
                TextBox1.Visible = False
            Case Keys.R
                PictureBox3.Image.RotateFlip(RotateFlipType.Rotate90FlipX)
                Me.Refresh()
        End Select
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        chase(PictureBox1)
    End Sub
    Sub move(P As PictureBox, x As Integer, y As Integer)
        P.Location = New Point(P.Location.X + x, P.Location.Y + y)
        If Collision(P, "Wall") Then
            MsgBox("you hit a wall")
        End If
    End Sub
    Sub follow(p As PictureBox)
        Static headstart As Integer
        Static c As New Collection
        c.Add(PictureBox3.Location)
        headstart = headstart + 1
        If headstart > 10 Then
            p.Location = c.Item(1)
            c.Remove(1)
        End If
    End Sub
    Public Sub chase(p As PictureBox)
        Dim x, y As Integer
        If p.Location.X > PictureBox3.Location.X Then
            x = -5
        Else
            x = 5
        End If
        MoveTo(p, x, 0)
        If p.Location.Y < PictureBox3.Location.Y Then
            y = 5
        Else
            y = -5
        End If
        MoveTo(p, x, y)
    End Sub
    Function Collision(p As PictureBox, t As String, Optional ByRef other As Object = vbNull)
        Dim col As Boolean
        For Each c In Controls
            Dim obj As Control
            obj = c
            If obj.Visible AndAlso p.Bounds.IntersectsWith(obj.Bounds) And obj.Name.ToUpper.Contains(t.ToUpper) Then
                col = True
                other = obj
            End If
        Next
        Return col
    End Function
    'Return true or false if moving to the new location is clear of objects ending with t
    Function IsClear(p As PictureBox, distx As Integer, disty As Integer, t As String) As Boolean
        Dim b As Boolean
        p.Location += New Point(distx, disty)
        b = Not Collision(p, t)
        p.Location -= New Point(distx, disty)
        Return b
    End Function
    'Moves and object (won't move onto objects containing  "wall" and shows green if object ends with "win"
    Sub MoveTo(p As PictureBox, distx As Integer, disty As Integer)
        If IsClear(p, distx, disty, "WALL") Then
            p.Location += New Point(distx, disty)
        End If
        Dim other As Object = Nothing
        If p.Name = "PictureBox3" And Collision(p, "WIN", other) Then
            Me.BackColor = Color.Green
            other.visible = False
            PictureBox2.Visible = True
            Me.Visible = False
            Dim f As New Form2
            f.ShowDialog()
            Me.Visible = True
            Return
        End If
        If p.Name = "PictureBox3" And Collision(p, "fries", other) Then
            ProgressBar1.Value = ProgressBar1.Value + 10
            score = score + 1
            other.visible = False
            Return
        End If
    End Sub
    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        MoveTo(Bullet, 5, 0)
        Score_Label.Text = score
    End Sub
    Sub MoveTo(p As String, distx As Integer, disty As Integer)
        For Each c In Controls
            If c.name.toupper.ToString.Contains(p.ToUpper) Then
                MoveTo(c, distx, disty)
            End If
        Next
    End Sub
End Class
