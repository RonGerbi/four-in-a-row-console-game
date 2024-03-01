namespace FourInARow.Core
{
    public struct GameModes
    {
        private eGameModes m_GameMode;

        public GameModes(eGameModes gameMode)
        {
            m_GameMode = gameMode;
        }

        public eGameModes GameMode
        {
            get
            {
                return m_GameMode;
            }
        }

        public static bool TryParse(string i_ToParse, out GameModes? o_ParsedGameMode)
        {
            bool succeededParsing = true;
            o_ParsedGameMode = null;

            if (i_ToParse.Equals("1"))
            {
                o_ParsedGameMode = new GameModes(eGameModes.Solo);
            }
            else if (i_ToParse.Equals("2"))
            {
                o_ParsedGameMode = new GameModes(eGameModes.OneVsOne);
            }
            else
            {
                succeededParsing = false;
            }

            return succeededParsing;
        }
    }
}
