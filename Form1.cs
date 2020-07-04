using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace CompiladorCono
{
    public partial class CompiladorCono : Form
    {
        int pos = 0;
      
        int[] v;
        int cont;
        int letras;
        String titulo_Arch;
        String txt_copiar;
        String resultado_lexico;
        String errores_lexicos;
        String[] palabras_reservadas ={ "main", "if", "then", "else", "end", "do", "while", "repeat", "until", "cin", "cout", "real", "int", "boolean","float" };
        String[] numeros = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };


        //sintactico
        int index_token = 0;
        int cont_errores = 0;
        String[] tokens;
        String[] tipo_token;
        String[] lineas_token;
        String[] errores_sintacticos;
        String error_s = "";
        TreeNode raiz;
        public CompiladorCono()
        {
            string[] args = Environment.GetCommandLineArgs(); 
            InitializeComponent();
            {

                titulo_Arch = "";
                label3.Text = "1";
                automata();
                this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
                this.richTextBox1.TextChanged += (ob, ev) =>
                {
                 //   richTextBox10.Focus();
                    pos = richTextBox1.SelectionStart;
                    automata();
                };
            }
            

        }
        public void automata() { //Pendiente hacer automata XD
            if (!string.IsNullOrEmpty(richTextBox1.Text.ToString()))
            {
                //auxiliar para hacer todo ahi y no se vea parpadeo
                var richTextBoxAux = new RichTextBox();
                label3.Focus();
                richTextBoxAux = richTextBox1;
                richTextBoxAux.SelectAll();
                richTextBoxAux.SelectionColor = Color.Black;
                richTextBoxAux.Select(richTextBoxAux.TextLength, 0);
                //el texto a buscar es lo que contiene el richtext
                String textoBuscador = richTextBoxAux.Text;
                string find = "";
                string find2 = "";
                int primerPintada = 0;
                foreach (string word in palabras_reservadas)
                {
                    find = word;
                    //si el richtext contiene alguna palabra reservada
                    if (richTextBoxAux.Text.Contains(find))
                    {
                        //
                        var matchString = Regex.Escape(find);
                        //match regresa todas las posiciones iniciales donde se haya encontrado alguna palabra ya encontrada en reserv
                        foreach (Match match in Regex.Matches(richTextBoxAux.Text, matchString))
                        {
                            //si donde se encontro no es la primer posicion
                            if (match.Index > 0)
                            {
                                //y el index de donde esta mas el tamaño de la reservada es menor al campototal
                                if (match.Index + find.Length < textoBuscador.Length)
                                {
                                    //y en la posicion anterior no es letra o digito o la sigiente
                                    if (!Char.IsLetterOrDigit(textoBuscador[match.Index - 1]) && !Char.IsLetterOrDigit(textoBuscador[match.Index + find.Length]))
                                    /*if ((textoBuscador[match.Index - 1] == '\n' || textoBuscador[match.Index - 1] == ' ' || textoBuscador[match.Index - 1] == '{' || textoBuscador[match.Index - 1] == '}'
                                        || textoBuscador[match.Index - 1] == ';' || textoBuscador[match.Index - 1] == ')') && (textoBuscador[match.Index + find.Length] == ' '
                                        || textoBuscador[match.Index + find.Length] == '(' || textoBuscador[match.Index + find.Length] == '{' || textoBuscador[match.Index + find.Length] == '\n'
                                        || textoBuscador[match.Index + find.Length] == '\0' || textoBuscador[match.Index + find.Length] == ';'))*/
                                    {
                                        richTextBoxAux.Select(match.Index, find.Length);
                                        richTextBoxAux.SelectionColor = Color.Blue;
                                        richTextBoxAux.Select(richTextBoxAux.TextLength, 0);
                                        richTextBoxAux.SelectionColor = richTextBoxAux.ForeColor;
                                    }
                                }

                            }
                            if (match.Index + find.Length == textoBuscador.Length && match.Index > 0)
                            {
                                if (find.Length == textoBuscador.Length && !Char.IsLetterOrDigit(textoBuscador[match.Index + find.Length]))
                                /*if (find.Length == textoBuscador.Length && (textoBuscador[match.Index + find.Length] == ' '
                                    || textoBuscador[match.Index + find.Length] == '(' || textoBuscador[match.Index + find.Length] == '{'
                                    || textoBuscador[match.Index + find.Length] == '\n' || textoBuscador[match.Index + find.Length] == '\0'
                                    || textoBuscador[match.Index + find.Length] == ';'))*/
                                {
                                    primerPintada = 1;
                                    richTextBoxAux.Select(match.Index, find.Length);
                                    richTextBoxAux.SelectionColor = Color.Blue;
                                    richTextBoxAux.Select(richTextBoxAux.TextLength, 0);
                                    richTextBoxAux.SelectionColor = richTextBoxAux.ForeColor;
                                }
                            }
                            else if (match.Index + find.Length < textoBuscador.Length) //&& primerPintada == 0)
                            {
                                if (find.Length < textoBuscador.Length && !Char.IsLetterOrDigit(textoBuscador[match.Index + find.Length]))
                                /*if (find.Length < textoBuscador.Length && (textoBuscador[match.Index + find.Length] == ' '
                                    || textoBuscador[match.Index + find.Length] == '(' || textoBuscador[match.Index + find.Length] == '{'
                                    || textoBuscador[match.Index + find.Length] == '\n' || textoBuscador[match.Index + find.Length] == '\0'
                                    || textoBuscador[match.Index + find.Length] == ';'))*/
                                {
                                    primerPintada = 1;
                                    richTextBoxAux.Select(match.Index, find.Length);
                                    richTextBoxAux.SelectionColor = Color.Blue;
                                    richTextBoxAux.Select(richTextBoxAux.TextLength, 0);
                                    richTextBoxAux.SelectionColor = richTextBoxAux.ForeColor;
                                }
                            }
                        };
                    }
                }


                // una linea

                find = "//";

                //si contiene algun // Error /*//
                if (richTextBoxAux.Text.Contains(find))
                {
                    var matchString = Regex.Escape(find);
                    //regresa cada posicion inicial de donde se encontro
                    foreach (Match match in Regex.Matches(richTextBoxAux.Text, matchString))
                    {
                        bool banderaSimple = true;
                        if (banderaSimple == true)
                        {
                            string texto = richTextBoxAux.Text;

                            //minimo se pintaran 2 caracteres del //
                            int busca = 2;
                            for (int i = match.Index + 1; texto[i] != '\n' && texto[i] != '\0' && i < texto.Length - 1; i++)
                            {
                                busca++;
                            }
                            richTextBoxAux.Select(match.Index, busca);
                            richTextBoxAux.SelectionColor = Color.Green;
                            richTextBoxAux.Select(pos, 0);

                        }
                    };
                }


                //Multiples Lineas

                find = "/*";

                //si contiene algun /* 
                if (richTextBoxAux.Text.Contains(find))
                {
                    var matchString = Regex.Escape(find);
                    //regresa cada posicion inicial de donde se encontro
                    foreach (Match match in Regex.Matches(richTextBoxAux.Text, matchString))
                    {
                        bool banderaSimple = true;
                        int comentarioLinea = 0;
                        if (banderaSimple == true)
                        {
                            string texto = richTextBoxAux.Text;
                            int primerCaracter = richTextBox1.GetFirstCharIndexFromLine(richTextBox1.GetLineFromCharIndex(match.Index));
                            for (int j = primerCaracter; j < match.Index + 1; j++)
                            {
                                if (j > primerCaracter && texto[j] == '/' && texto[j - 1] == '/')
                                {
                                    comentarioLinea = 1;
                                }
                            }
                            if (comentarioLinea == 0)
                            {
                                //minimo se pintaran 2 caracteres del /*
                                int busca = 2;
                                for (int i = match.Index + 1; i < texto.Length - 1; i++)
                                {
                                    if (texto[i - 1] == '*' && texto[i] == '/' && i > match.Index + 2)
                                    {
                                        break;
                                    }
                                    busca++;
                                }
                                richTextBoxAux.Select(match.Index, busca);
                                richTextBoxAux.SelectionColor = Color.Green;
                                richTextBoxAux.Select(pos, 0);

                            }
                        }
                    };
                }



                richTextBox1 = richTextBoxAux;

                if (pos >= 0)
                {
                    richTextBox1.Select(pos, 0);
                }
                richTextBox1.Focus();
            }

        }


        
    
       
       
        public void actualizaCursor()
        {
           
            int cont = 1;
            int cont2 = 0;
            for (int i = 0; i < richTextBox1.Text.Length; i++)
            {
                cont2++;
                if (i == richTextBox1.SelectionStart)
                {
                    break;
                }
                if (richTextBox1.Text[i] == '\n')
                {
                    cont++;
                    cont2 = 0;
                }


            }
            String linea = "F: " + cont + " C: " + (cont2+1);
            label2.Text = linea;

        }
       

        private void actualizarLabel()
        {
     
            Point xy = new Point(0, 0);
            int pI = richTextBox1.GetCharIndexFromPosition(xy);
            int pL = richTextBox1.GetLineFromCharIndex(pI);
            xy.X = ClientRectangle.Width;
            xy.Y = ClientRectangle.Height;
            int uI = richTextBox1.GetCharIndexFromPosition(xy);
            int uL = richTextBox1.GetLineFromCharIndex(uI+1);
            xy = richTextBox1.GetPositionFromCharIndex(uI);
            label3.Text = "";
            for (int i = pL+1; i <= uL+1; i++)
            {
                label3.Text += i + "\n";
            }
        } 
        public void abrir()
        {
            String text = "";
            openFileDialog1.ShowDialog();
            System.IO.StreamReader file = new System.IO.StreamReader(openFileDialog1.FileName);
            text = file.ReadToEnd();
            richTextBox1.Text = text.ToString();
            titulo_Arch = openFileDialog1.FileName;
            //label2.Text = titulo_Arch;
            file.Close();
          ///  richTextBox1.SelectionStart = 0;
            richTextBox1.Focus();
        }
        public void abrir_archivo()
        {
          
            if (richTextBox1.Text != "" || titulo_Arch != "") //Hay algo
            {
                DialogResult boton = MessageBox.Show("Guardar Documento?", "Alerta", MessageBoxButtons.YesNoCancel);
                if (boton == DialogResult.Yes)
                {
                    guardar();
                    abrir();
                }
                if(boton == DialogResult.No)
                {
                    abrir();
                }
                

            }
            else
            {
                abrir();
            }

        }
        public void guardar_archivo()
        {
            saveFileDialog1.FileName = "Nombre del Archivo.txt";
            var s = saveFileDialog1.ShowDialog();
            if(s == DialogResult.OK)
            {
                using (var SaveFile = new System.IO.StreamWriter(saveFileDialog1.FileName))
                {
                    SaveFile.Write(richTextBox1.Text);
                    
                }
            }
            titulo_Arch = saveFileDialog1.FileName;
            label9.Text = "";
        }
        public void guardar()
        {
            if(titulo_Arch == "")
            {
                guardar_archivo();
            }
            else
            {
                
                using (var SaveFile = new System.IO.StreamWriter(titulo_Arch))
                {
                    SaveFile.Write(richTextBox1.Text);
                }
            }
            label9.Text = "";


        }
        public void inicializar_rich()
        {
            titulo_Arch = "";
            richTextBox1.Text = "";
          //  richTextBox1.SelectionStart = 0;
            richTextBox1.Focus();
            actualizarLabel();
            actualizaCursor();
            label9.Text = "";
        }
        public void nuevo_archivo()
        {
            richTextBox1.Enabled = true;
            if (richTextBox1.Text != "") //Hay algo
            {
                DialogResult boton = MessageBox.Show("Guardar Documento?", "Alerta", MessageBoxButtons.YesNoCancel);
                if (boton == DialogResult.Yes)
                {
                    guardar();
                    inicializar_rich();
                }
                if(boton == DialogResult.No)
                {
                    inicializar_rich();
                }

            }
           
        }


        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {
            actualizarLabel();
            actualizaCursor();
            label9.Text = "*";
          
        }

        private void richTextBox1_VScroll(object sender, EventArgs e)
        {
            actualizarLabel();
            
        }

       

        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                actualizaCursor();
            }
            if(e.KeyCode == (Keys.Shift | Keys.NumPad7) || e.KeyCode == Keys.Enter){
            }
         
          if(e.KeyCode == Keys.Enter)
            {
                actualizaCursor();
            }

            
        }

        private void richTextBox1_Click(object sender, EventArgs e)
        {
          
            actualizaCursor();
           
        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStrip1_ItemClicked_1(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void archivoToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void abrirToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            abrir_archivo();
        }

        private void guardarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            guardar();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void nuevoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            nuevo_archivo();
        }

        private void label4_Click(object sender, EventArgs e)
        {
            nuevo_archivo();
        }

        private void guardarComoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            guardar_archivo();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            abrir_archivo();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            guardar();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            DialogResult boton = MessageBox.Show("Cerrar Aplicación?", "Alerta", MessageBoxButtons.YesNo);
            if (boton == DialogResult.Yes)
            {
                if(titulo_Arch != "" || richTextBox1.Text != "")
                {
                    DialogResult boton2 = MessageBox.Show("Guardar Archivo?", "Alerta", MessageBoxButtons.YesNo);
                    if (boton2 == DialogResult.Yes)
                    {
                        guardar();
                        this.Close();
                    }
                    if (boton2 == DialogResult.No)
                    {
                        this.Close();

                    }  
                }
                else
                {
                    this.Close();
                }
                
            }
            
        }

        
        private void label8_Click(object sender, EventArgs e)
        {
            guardar_archivo();
        }
        private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(richTextBox1.SelectedText);
            }
            catch(Exception ex)
            {

            }


        }

        private void pegarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                txt_copiar = Clipboard.GetText();
                int n = richTextBox1.SelectionStart;
                richTextBox1.Text = richTextBox1.Text.Insert(n, txt_copiar);
            }catch(Exception ex)
            {

            }
          
        }

        private void cortarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(richTextBox1.SelectedText);
                richTextBox1.SelectedText = "";
            }catch(Exception ex)
            {

            }

            
        }

        private void eliminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        private void léxicoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            guardar();
            Lexico lexico = new Lexico(titulo_Arch);
            lexico.Abrir_grafico(richTextBox1.Text);
            resultado_lexico= lexico.Analizador_Lexico();
            richTextBox2.Text = resultado_lexico;
            errores_lexicos = "";
            for(int i = 0; i < lexico.cont_errores_l; i++)
             {
                errores_lexicos += lexico.errores[i];
             }
            richTextBox8.Text = errores_lexicos;
            lexico.guardar_archivo(resultado_lexico, titulo_Arch);
            lineas_token = lexico.lineas_tokens;
        }

        private void léxicoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            léxicoToolStripMenuItem1_Click(sender,e);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            sintácticoToolStripMenuItem_Click(sender,e);
        }

        private void sintácticoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            léxicoToolStripMenuItem1_Click(sender, e);
            //  Sintactico s = new Sintactico(resultado_lexico);
           index_token = 0;
            String[] pal = resultado_lexico.Split('\n');
            tokens = new String[pal.Length];
            tipo_token = new string[pal.Length];
            String txt = "";
            try
            {
                for (int i = 0; i < pal.Length; i++)
                {
                    tokens[i] = pal[i].Split(' ')[2];
                    tipo_token[i] = pal[i].Split(' ')[0];
                   // txt += tokens[i] + i + " \n";
                }
            }
            catch (Exception ex) { }
            //richTextBox4.Text = txt;
            treeView1.Nodes.Clear();

            inicio_sintactico();
            try
            {
                treeView1.Nodes.Add(raiz);
                treeView1.ExpandAll();
               
            }
            catch (Exception ex) { }
            richTextBox11.Text =  error_s;
            error_s = "";
               
        }
        public void error()
        {
        }
        public void add_error(String s)
        {
            int b = 0;
            for(int i = 0; i < cont_errores; i++)
            {
                if (s == errores_sintacticos[i]) b = 1;
            }
            if( b == 0)
            {
                errores_sintacticos[cont_errores] = s;
                error_s += s+"\n";
                cont_errores++;
            }
        }

        public bool match(String t)
        {
            if(tokens[index_token] == t)
            {
                index_token++;
                return true;
            }
            else
            {
                if(index_token > 0)
                {
                   add_error("Se esperaba un '" + t + "' en " + lineas_token[index_token - 1]);
                }
               
                error();
               
            }
            return false;
        }
        public void inicio_sintactico()
        {
            errores_sintacticos = new String[20000];
            cont_errores = 0;
            raiz = programa();
        }
        public TreeNode programa()
        {
            TreeNode temp=null;
            match("main");
            match("{");
            temp = new TreeNode("main");
            temp.Nodes.Add(lista_declaracion());
            temp.Nodes.Add(lista_sentencias());
            match("}");
            return temp;
        }
        public TreeNode lista_declaracion()
        {
            TreeNode temp = null;
            temp = new TreeNode("list-decl");
            temp.Nodes.Add(declaracion());
            match(";");
            while (tipo_token[index_token] == "palabra_reservada")
            {
                temp.Nodes.Add(declaracion());

            }
            return temp;
        }
        public TreeNode declaracion()
        {
            TreeNode temp = null;
            temp = new TreeNode("decl");
            temp.Nodes.Add(tokens[index_token]);
            switch (tokens[index_token])
            {
                case "int":
                    match("int");
                    temp.Nodes[0].Nodes.Add(lista_variables());
                    break;
                case "real":
                    match("real");
                    temp.Nodes[0].Nodes.Add(lista_variables());
                    break;
                case "boolean":
                    match("boolean");
                    temp.Nodes[0].Nodes.Add(lista_variables());
                    break;
                case "float":
                    match("float");
                    temp.Nodes[0].Nodes.Add(lista_variables());
                    break;
            }
            return temp;
        }
        public TreeNode lista_variables()
        {
            TreeNode temp = null;
            TreeNode nuevo = null;
            int cont = 0;
            temp = new TreeNode("list-var");
            while(tipo_token[index_token] == "id" )
            {
                temp.Nodes.Add(tipo_token[index_token]);
                temp.Nodes[cont].Nodes.Add(tokens[index_token]);
                index_token++;
                cont++;
                if (tokens[index_token] == "," && tipo_token[index_token+1] == "id") match(",");
                else
                {
                    match(";");
                    break;
                }
            }
            return temp;
        }
        public TreeNode lista_sentencias()
        {
            TreeNode temp = null;
            temp = new TreeNode("sent-list");
            temp.Nodes.Add(sentencia());
            while(tipo_token[index_token] == "id" || (tipo_token[index_token] == "palabra_reservada"  && tokens[index_token] != "until" && tokens[index_token] != "end" && tokens[index_token] !="else" ) || tokens[index_token] == "{" )
            {
                temp.Nodes.Add(sentencia());
            }
            return temp;
        }  
        public TreeNode sentencia()
        {
            TreeNode temp = null;
            temp = new TreeNode("sent");
            switch (tokens[index_token])
            {
                case "if":
                    temp.Nodes.Add(seleccion());
                break;
                case "while":
                    temp.Nodes.Add(iteracion());
                break;
                case "do":
                    temp.Nodes.Add(repeat());
                    break;
                case "cin":
                    temp.Nodes.Add(sentCin());
                    match(";");
                    break;
                case "cout":
                    temp.Nodes.Add(sentCout());
                    match(";");
                    break;
                case "{":
                    temp.Nodes.Add(Bloque2());
                    break;
                default:
                    if(tipo_token[index_token] == "id")
                    {
                        try
                        {
                            temp.Nodes.Add(asignacion());
                        }
                        catch(Exception ex) { }
                    }
                    else
                    {

                    }    
                break;
            }
            return temp;
        }
        public TreeNode seleccion()
        {
            TreeNode temp = null;
            TreeNode nuevo = new TreeNode("if");

            match("if");
            temp = expresion();
            match("then");

            nuevo.Nodes.Add("sent-if");
            nuevo.Nodes.Add(Bloque());
            nuevo.Nodes[0].Nodes.Add(temp);
            if (tokens[index_token] == "else")
            {
                match("else");
                nuevo.Nodes.Add("sent-else");
                nuevo.Nodes[2].Nodes.Add(Bloque());
            }
            match("end");
            match(";");
            temp = nuevo;
            return temp;
        }
        public TreeNode iteracion()
        {
            TreeNode temp = null;
            TreeNode nuevo = null;
          
            match("while");
            temp = expresion();
            nuevo = new TreeNode("sent-while");
            nuevo.Nodes.Add(temp);
            nuevo.Nodes.Add(Bloque2());
            temp = nuevo;
            return temp;
        }
        public TreeNode repeat()
        {
            TreeNode temp = null;
            TreeNode nuevo = null;
            match("do");
            temp = Bloque();
            match("until");
            nuevo = new TreeNode("sent-repeat");
            nuevo.Nodes.Add(temp);
            nuevo.Nodes.Add("until");
            nuevo.Nodes[1].Nodes.Add(expresion());
            match(";");
            temp = nuevo;
            return temp;
        }
        public TreeNode sentCin()
        {
            TreeNode temp = null;
            match("cin");
            if (tipo_token[index_token] == "id")
            {
                temp = new TreeNode("cin");
                temp.Nodes.Add(tipo_token[index_token]); 
                temp.Nodes[0].Nodes.Add(tokens[index_token]);
                index_token++;
            }
            else
            {
                error();
            }
            return temp;
        }
        public TreeNode sentCout()
        {
            TreeNode temp = null;
            match("cout");
            if (tipo_token[index_token] == "id")
            {
                temp = new TreeNode("cout");
                temp.Nodes.Add(tipo_token[index_token]);
                temp.Nodes[0].Nodes.Add(tokens[index_token]);
                index_token++;
            }
            else
            {
                error();
            }
            return temp;
        }
        public TreeNode Bloque2()
        {
            TreeNode temp = null;
            match("{");
            temp = lista_sentencias();
            match("}");
            return temp;
        }
        public TreeNode Bloque()
        {
            TreeNode temp = null;
            temp = lista_sentencias();
            return temp;
        }

        public TreeNode asignacion()
        {
            TreeNode temp = null;
            String tok = "";
            if (tipo_token[index_token] == "id")
            {
                tok = tokens[index_token];
                index_token++;
                if (tokens[index_token] == "++" || tokens[index_token] == "--")
                {
                    temp = new TreeNode(tokens[index_token]);
                    index_token--;
                    temp.Nodes.Add(expresion());
                }
                else{ 
                    if (match(":="))
                    {
                        temp = new TreeNode(":=");
                        temp.Nodes.Add("id");
                        temp.Nodes[0].Nodes.Add(new TreeNode(tok));
                        temp.Nodes.Add(expresion());
                    }
                    else
                    {
                        while (tokens[index_token] != ";") index_token++;
                        error();
                    }
                }
                if (!match(";"))
                {
                    while (tokens[index_token] != ";"  ) index_token++;
                    match(";");
                }
            }
            return temp;
        }

        public TreeNode expresion()
        {
            TreeNode temp = null;
            TreeNode nuevo = null;
            temp = expresion_simple();
            try
            {
                while (tokens[index_token] == "<=" || tokens[index_token] == "<" || tokens[index_token] == ">" || tokens[index_token] == ">="
                        || tokens[index_token] == "==" || tokens[index_token] == "!=")
                {
                    switch (tokens[index_token])
                    {
                        case "<=":
                            match("<=");
                            nuevo = new TreeNode("<=");
                            nuevo.Nodes.Add(temp);
                            nuevo.Nodes.Add(expresion_simple());
                            temp = nuevo;
                            break;
                        case "<":
                            match("<");
                            nuevo = new TreeNode("<");
                            nuevo.Nodes.Add(temp);
                            nuevo.Nodes.Add(expresion_simple());
                            temp = nuevo;
                            break;
                        case ">":
                            match(">");
                            nuevo = new TreeNode(">");
                            nuevo.Nodes.Add(temp);
                            nuevo.Nodes.Add(expresion_simple());
                            temp = nuevo;
                            break;
                        case ">=":
                            match(">=");
                            nuevo = new TreeNode(">=");
                            nuevo.Nodes.Add(temp);
                            nuevo.Nodes.Add(expresion_simple());
                            temp = nuevo;
                            break;
                        case "==":
                            match("==");
                            nuevo = new TreeNode("==");
                            nuevo.Nodes.Add(temp);
                            nuevo.Nodes.Add(expresion_simple());
                            temp = nuevo;
                            break;
                        case "!=":
                            match("!=");
                            nuevo = new TreeNode("!=");
                            nuevo.Nodes.Add(temp);
                            nuevo.Nodes.Add(expresion_simple());
                            temp = nuevo;
                            break;
                    }
                }
            }
            catch (Exception e) { }
            return temp;
        }
        public TreeNode expresion_simple()
        {
            TreeNode temp = null;
            TreeNode nuevo = null;
            String m = "";
            temp = termino();
            try
            {
                while (tokens[index_token] == "+" || tokens[index_token] == "-" || tokens[index_token] == "++" || tokens[index_token] == "--")
                {
                    switch (tokens[index_token])
                    {
                        case "+":
                            match("+");
                            nuevo = new TreeNode("+");
                            nuevo.Nodes.Add(temp);
                            nuevo.Nodes.Add(termino());
                            temp = nuevo;
                            break;
                        case "-":
                            match("-");
                            nuevo = new TreeNode("-");
                            nuevo.Nodes.Add(temp);
                            nuevo.Nodes.Add(termino());
                            temp = nuevo;
                            break;
                        case "++":
                            match("++");
                            nuevo = new TreeNode("+");
                            nuevo.Nodes.Add(temp);
                            nuevo.Nodes.Add("factor");
                            nuevo.Nodes[1].Nodes.Add("1");
                            temp = nuevo;
                            break;
                        case "--":
                            match("--");
                            nuevo = new TreeNode("-");
                            nuevo.Nodes.Add(temp);
                            nuevo.Nodes.Add("factor");
                            nuevo.Nodes[1].Nodes.Add("1");
                            temp = nuevo;
                        break;
                    }
                }
            }catch(Exception e) { }
         
            return temp;
        }
        public TreeNode termino()
        {
            TreeNode temp = null;
            TreeNode nuevo = null;
            temp = factor();
            try
            {
                while(tokens[index_token] == "*" || tokens[index_token] == "/" || tokens[index_token] == "%")
                {
                    switch (tokens[index_token])
                    {
                        case "*":
                            match("*");
                            nuevo = new TreeNode("*");
                            nuevo.Nodes.Add(temp);
                            nuevo.Nodes.Add(factor());
                            temp = nuevo;
                            break;
                        case "/":
                            match("/");
                            nuevo = new TreeNode("/");
                            nuevo.Nodes.Add(temp);
                            nuevo.Nodes.Add(factor());
                            temp = nuevo;
                            break;
                        case "%":
                            match("%");
                            nuevo = new TreeNode("%");
                            nuevo.Nodes.Add(temp);
                            nuevo.Nodes.Add(factor());
                            temp = nuevo;
                            break;
                    }
                }
              
            }catch(Exception ex) { }
          
    
            return temp;
        }
        public TreeNode factor()
        {
            TreeNode temp = null;
            TreeNode nuevo = null;
            temp = fin();
            try
            {
                if (tokens[index_token] == "^")
                {
                    match("^");
                    nuevo = new TreeNode("^");
                    nuevo.Nodes.Add(temp);
                    nuevo.Nodes.Add(fin());
                    temp = nuevo;
                }
            }catch(Exception ex) { }
            return temp;
        }
        public TreeNode fin()
        {
            double numero = 0;
            TreeNode temp = null;
            try
            {

                if (tokens[index_token] == "(")
                {
                    match("(");
                    temp = expresion();
                    match(")");
                }
                else if (double.TryParse(tokens[index_token], out numero) || tipo_token[index_token] == "id")
                {
                    temp = new TreeNode("factor");
                    temp.Nodes.Add(new TreeNode(tokens[index_token] + ""));
                    index_token++;
                }
                else
                {
                    error();
                }
            }
            catch (Exception ex) { }
            return temp;
        }

        private void tabPage8_Click(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
