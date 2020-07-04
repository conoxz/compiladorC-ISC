using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompiladorCono
{
    class Lexico
    {
        
        String fileName;
        String contenidoFile;
        String analizador;
        public String[] errores;
        public String[] lineas_tokens;
        public int cont_lineas;
        public int cont_errores_l;
        enum estados
        {
            inicio,
            id,
            mas,
            menos,
            num,
            posible_float,
            num_float,
            menor,
            mayor,
            igual,
            chido,
            operacion,
            posible_com_line,
            coment_line,
            posible_com_inline,
            posible_fin_com_inline,
            hecho
        }
        //Diccionarios
        /*  IDictionary<int, char> letras = new Dictionary<int, char>();
        IDictionary<int, char> digito = new Dictionary<int, char>();
        IDictionary<int, char> varios = new Dictionary<int, char>();
        IDictionary<int, char> otros_signos = new Dictionary<int, char>();
        IDictionary<int, char> previo = new Dictionary<int, char>(); */
        char[] letras = new char[100];
        char[] digito = new char[100];
        char[] varios = new char[100];
        char[] previo = new char[100];
        char[] otros_signos = new char[100];

        char[] signos = { '-', '+', '*', '/', '%', '=', '!','>','<',':',';','(',')',',','[',']','{','}','^'};
        String[] palabras_reservadas = { "main", "if", "then", "else", "end", "do", "while", "repeat", "until", "cin", "cout", "real", "int", "boolean","float" };
        public Lexico(String str)
        {
            errores = new String[2000000];
            lineas_tokens = new String[2000000];
            cont_lineas = 0;
            this.fileName = str;
            this.analizador = "";
            //letras
            for(int  i = 0; i <=25; i++)
            {
                letras[i] = (char)(i+65); //mayusculas A-Z
                letras[i] = (char)(i+97); //minusculas a-z
                varios[i+26] = (char)(i+65);
                varios[i+26] = (char)(i+97);
              // Console.WriteLine((char)(i + 65) + " " + (char)(i + 97));}
            }
            varios.Append((char)95); //guion_bajo
            //digitos
            for(int i = 0; i < 10; i++)
            {
                digito[i] = (char)(i+48); //0-9
                varios[i] = (char)(i+48);
               // Console.WriteLine(i+48);
            }
            //previo
            for(int i= 0; i < 33; i++)
            {
                previo[i] = ((char)i);
            }
            //otros_signos
            otros_signos[0] = ((char)40);
            otros_signos[1] = ((char)41);
            otros_signos[2] = ((char)44);
            otros_signos[3] = ((char)123);
            otros_signos[4] = ((char)125);
            otros_signos[5] = ((char)59);
            otros_signos[6] = ((char)91);
            otros_signos[7] = ((char)93);

        }
        
        public Boolean palabra_igual(String str)
        {
            if (palabras_reservadas.Contains(str)){
                return true;
            }
            
            return false;
        }

        public String Abrir()
        {
            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(fileName);
                contenidoFile = file.ReadToEnd();
                file.Close();
                
            }
            catch (Exception e)
            {
                contenidoFile = "Error al abrir archivo";
            }
            
            return contenidoFile;
        }
        public void Abrir_grafico(String str)
        {
            contenidoFile = str;
        }
        public string error_linea(String str, int n){
            int cont = 1;
            int cont2 = 0;
            for(int  i = 0; i < str.Length; i++)
            {
                cont2++;
                if (i == n)
                {
                    break;
                }
                if(str[i] == '\n'){
                    cont++;
                    cont2=0;
                }
                
               
              
            }
            String linea = "fila: "+cont+",columna: "+(cont2);
            return linea;
        }
        public String Analizador_Lexico()
        {
            char[] analizador_char = contenidoFile.ToCharArray();
            string[] errores;
            int i = 0;
            int estado;
            int cont_errores = 0;
            String acum_estados = "";
            String que_es = "";
            Boolean b1 = true;
            while ( i < analizador_char.Length) //tokens
            {
                estado = (int)estados.inicio;
                acum_estados = "";
                int linea;
                int columna;
                while(estado != (int)estados.hecho) { //automata finito
                    b1 = true;
                    switch (estado)
                    {
                        case (int)estados.inicio:
                            if (letras.Contains(analizador_char[i])) {//to id
                                estado = (int)estados.id;
                                que_es = "id => ";
                            } else if (digito.Contains(analizador_char[i])) { //to num     
                                 estado = (int)estados.num;
                            } else if (analizador_char[i] == '+') { //to mas
                                estado = (int)estados.mas;
                            } else if (analizador_char[i] == '-') { //to menos
                                estado = (int)estados.menos;
                            } else if (analizador_char[i] == '<') {
                                estado = (int)estados.menor;
                            } else if (analizador_char[i] == '>') {
                                estado = (int)estados.mayor;
                            } else if (analizador_char[i] == ':' || analizador_char[i] == '=' || analizador_char[i] == '!') {
                                estado = (int)estados.igual;
                            }
                            else if (analizador_char[i] == '/')
                            {
                                estado = (int)estados.posible_com_line; //comentarios pendientes
                            }
                            else if (otros_signos.Contains(analizador_char[i])) {
                                estado = (int)estados.chido;
                            } else if (analizador_char[i] == '*' || analizador_char[i] == '%' || analizador_char[i] == '^') {
                                estado = (int)estados.operacion;
                            }
                             else if (previo.Contains(analizador_char[i])) {
                               //Console.WriteLine((int)analizador_char[i]);
                                i++;
                                b1 = false;
                                estado = (int)estados.hecho;
                              
                            } else {

                                que_es = "error => ";
                                acum_estados = "caracter no identificado '"+analizador_char[i]+"' "+error_linea(contenidoFile,i);
                                i++;
                                b1 = false;
                                estado = (int)estados.hecho;
                                this.errores[cont_errores] = acum_estados+"\n";
                                cont_errores++;
                            }  

                            if (b1){
                                acum_estados += analizador_char[i];
                                i++;                           
                            }
                         
                        break;
                        
                        case (int)estados.id:
                            if (varios.Contains(analizador_char[i])){
                                acum_estados += analizador_char[i];
                                i++;
                                estado = (int)estados.id;
                                que_es = "id => ";
                            }
                            else //error
                            {
                                estado = (int)estados.hecho;
                                que_es = "id => ";
                            }
                        break;
                        case (int)estados.num:
                            if (digito.Contains(analizador_char[i]))
                            {
                                acum_estados += analizador_char[i];
                                i++;
                                que_es = "entero => ";
                                estado = (int)estados.num;
                            }
                            else if(analizador_char[i] == '.')
                            {
                                acum_estados += analizador_char[i];
                                i++;
                                estado = (int)estados.num_float;
                            }
           
                            else //error
                            {
                                que_es = "entero => ";
                                estado = (int)estados.hecho;
                                if (!previo.Contains(analizador_char[i]) && !signos.Contains(analizador_char[i]))
                                {
                                    estado = (int)estados.hecho;
                                    this.errores[cont_errores] = "se esperaba un digito en " + error_linea(contenidoFile, i) + "\n";
                                    cont_errores++;
                                }
                            }
                            break;
                        case (int)estados.num_float:
                            if (digito.Contains(analizador_char[i]))
                            {
                                acum_estados += analizador_char[i];
                                i++;
                                que_es = "float => ";
                                estado = (int)estados.num_float;
                            }
                            else 
                            {
                                que_es = "float => ";
                                estado = (int)estados.hecho;
                                if (!previo.Contains(analizador_char[i]) && !signos.Contains(analizador_char[i]))
                                {
                                    estado = (int)estados.hecho;
                                    this.errores[cont_errores] = "se esperaba un digito en "+error_linea(contenidoFile, i) + "\n";
                                    cont_errores++;
                                }

                            } 

                           
                        break;
                        case (int)estados.mas:
                            if(analizador_char[i] == '+')
                            {
                                que_es = "incremento => ";
                                acum_estados += analizador_char[i];
                                i++;
                                estado = (int)estados.hecho;
                            }
                            else
                            {
                                que_es = "operador => ";
                                estado = (int)estados.hecho;
                            }
                        break;
                        case (int)estados.menos:
                            if(analizador_char[i] == '-'){
                                que_es = "decremento => ";
                                acum_estados += analizador_char[i];
                                estado = (int)estados.hecho;
                                i++;
                            }
                            else {
                                que_es = "operador => ";
                                estado = (int)estados.hecho;
                            }
                        break;
                        case (int)estados.menor: 
                            if(analizador_char[i] == '='){
                                que_es = "comparacion => ";
                                acum_estados += analizador_char[i];
                                estado = (int)estados.hecho;
                                i++;
                            }
                            else{
                                que_es = "menor => ";
                                estado = (int)estados.hecho;
                            }
                        break;
                        case (int)estados.mayor: 
                            if(analizador_char[i] == '='){
                                que_es = "Comparacion => ";
                                acum_estados += analizador_char[i];
                                estado = (int)estados.hecho;
                                i++;
                            }
                            else{
                                que_es = "mayor => ";
                                estado = (int)estados.hecho;
                            }
                        break;
                        case (int)estados.igual:
                            if (analizador_char[i] == '=')
                            {
                                if(analizador_char[i-1] == '='){
                                    que_es = "comparacion => ";
                                }
                                if (analizador_char[i - 1] == ':'){
                                    que_es = "asignacion => ";
                                }
                                if (analizador_char[i - 1] == '!'){
                                    que_es = "negacion => ";
                                }
                                acum_estados += analizador_char[i];
                                estado = (int)estados.hecho;
                                i++;
                            }
                            else {
                                que_es = "error => ";
                                estado = (int)estados.hecho;
                                acum_estados = "Se esperaba un '=' en "+ error_linea(contenidoFile, i-1);
                                this.errores[cont_errores] = acum_estados + "\n";
                                cont_errores++;
                            }
                        break;
                        case (int)estados.chido:
                            que_es = "caracter_simple => ";
                            estado = (int)estados.hecho; 
                        break;
                        case (int)estados.operacion:
                            que_es = "operador => ";
                            estado = (int)estados.hecho;
                        break;
                        case (int)estados.posible_com_line:
                            que_es = "";
                            acum_estados = "";
                            if(analizador_char[i] == '/')
                            {
                                i++;
                                estado = (int)estados.coment_line;
                            }
                            else if(analizador_char[i] == '*')
                            {
                                i++;
                                estado = (int)estados.posible_com_inline;
                            }
                            else { 

                                que_es = "operador => ";
                                acum_estados = "/";
                                estado = (int)estados.hecho; 
                            }
                            
                        break;
                        case (int)estados.coment_line: 
                            if( analizador_char[i] == '\n') {
                                i++;
                                estado = (int)estados.hecho;
                            }
                            else{
                                estado = (int)estados.coment_line;  
                                i++;
                            }

                        break;
                        case (int)estados.posible_com_inline:
                            if (analizador_char[i] == '*')
                            {
                                i++;
                                estado = (int)estados.posible_fin_com_inline;
                            }
                            else
                            {
                                i++;
                                estado = (int)estados.posible_com_inline;
                            }
                        break;
                        case (int)estados.posible_fin_com_inline:
                            if(analizador_char[i] == '/'){
                                i++;
                                estado = (int)estados.hecho;
                            }
                            else {
                                i++;
                                estado = (int)estados.posible_com_inline;
                            }
                        break;
                        case (int)estados.hecho:
                            estado = (int)estados.inicio;
                        break;

                    }
                    if (i >= analizador_char.Length) break;
                }
                if (palabra_igual(acum_estados)) 
                    que_es = "palabra_reservada => ";
                if( (que_es.Length>0 && acum_estados.Length>0 || i >= analizador_char.Length) && que_es != "error => ")
                {
                    // Console.WriteLine(que_es+acum_estados);
                    analizador += que_es + acum_estados + "\n";
                    que_es = "";
                    acum_estados = "";
                    lineas_tokens[cont_lineas] = error_linea(contenidoFile, i);
                    cont_lineas++;
                }
                if (i >= analizador_char.Length) break;

            }
            this.cont_errores_l = cont_errores;
            return analizador;
        }
        public void guardar_archivo(String str,String titulo) {
            String []tit = titulo.Split('\\');
            //Console.WriteLine(tit[tit.Length - 1]);
            String fecha = DateTime.Now.ToString("dd:MM:yyyy:hh:mm:ss.f");
            String name = "lexico-"+ tit[tit.Length - 1];
            try
            {   
                System.IO.File.WriteAllText(@"C:\Users\usuario\Desktop\8oSemestre\Compiladores\CompiladorCono\resultados\"+name, str);
            }
            catch(Exception e)
            {
                Console.WriteLine("Error al crear archivo");
            }
        }


    }
}













