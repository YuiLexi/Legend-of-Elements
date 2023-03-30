public static class OverAllVariable
{
    private static int _sunNum = 100;

    public static int SunNum
    {
        get
        {
            return _sunNum;
        }
        set
        {
            if (value < 0)
                value = 0;
            _sunNum = value;
        }
    }
}