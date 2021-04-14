using System;
using System.Text;

namespace AppNotas
{
    class Test
    {
        public static string changeFont1(string firstName)
        {
             StringBuilder newText2 = new StringBuilder();
          //  string newText2 = string.Empty;
            int textLenght = firstName.Length;
            int aux = 0;

            for (int i = 1; i <= textLenght; i++)
            {
                if (i % 2 == 0)
                {
                    aux = -5;
                }
                else if (i % 3 == 0)
                {
                    aux = -3;
                }
                //  newChar = (char)((text[i - 1]) + aux);
                newText2.Append((char)((firstName[i - 1]) + aux));
              //  newText2 += (char)((firstName[i - 1]) + aux);
            }

            return newText2.ToString();
        }


        public static string changeFont(string lastName)
        {
            StringBuilder newText2 = new StringBuilder();
            int textLenght = lastName.Length;
            int aux = 0;
            //    Char newChar;

            for (int i = 1; i <= textLenght; i++)
            {
                if (i % 2 == 0)
                {
                    aux = -5;
                }
                else if (i % 3 == 0)
                {
                    aux = -3;
                }

                //  newChar = (char)((text[i - 1]) - aux);
                newText2.Append((char)((lastName[i - 1]) - aux));
            }

            return newText2.ToString();
        }


        // PASS ENCRYPT !

        public static string changeFont2(string word)
        {
            //StringBuilder newPass1 = new StringBuilder();
            string newPass1 = string.Empty;
            char newChar;
            int passLenght = word.Length;
            int aux = 0;
            for (int i = 0; i < passLenght; i++)
            {
                if (i % 3 == 0)
                {
                    aux = 1;
                  //  newPass1.Append(word[i] - i);
                }
                else
                {
                    aux = 2;
                }
                newChar = (char)(word[i] - aux);
             //   newPass1.Append(newChar);
                newPass1 += newChar;
            }
            return newPass1;
        }
    }
}
