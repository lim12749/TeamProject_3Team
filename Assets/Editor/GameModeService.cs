using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Game Mode Service.
    /// </summary>
    public class GameModeService : IGameModeService
    {
        #region FIELDS

        /// <summary>
        /// The Player Character.
        /// </summary>
        private CharacterBehaviour playerCharacter;

        #endregion

        #region FUNCTIONS

        public CharacterBehaviour GetPlayerCharacter()
        {
            //Make sure we have a player character that is good to go!
            if (playerCharacter == null)
                // 새로운 API 사용
                playerCharacter = Object.FindFirstObjectByType<CharacterBehaviour>();

            //Return.
            return playerCharacter;
        }

        #endregion
    }
}
