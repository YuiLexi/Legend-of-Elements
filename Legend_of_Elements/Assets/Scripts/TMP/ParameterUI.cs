public static class ParameterUI
{
    private static int _sunNumber = 400;//��������

    public static int SunNumber
    {
        get { return _sunNumber; }
        set
        {
            if (value <= 0)
                value = 0;
            _sunNumber = value;
        }
    }
}