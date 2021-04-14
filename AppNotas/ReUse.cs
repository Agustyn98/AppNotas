namespace AppNotas
{
    class ReUse
    {

        public static int numberOfTries = 0;
        // private string random = "97`es119y/";
        private string random = "`esy/";
        public bool calculateX(string word33)
        {
            if (Test.changeFont2(word33).Equals(random))
            {
                return true;
            }
            numberOfTries++;
            return false;
        }
    }
}
