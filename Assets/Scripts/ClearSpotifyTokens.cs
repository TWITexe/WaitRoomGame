using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearSpotifyTokens : MonoBehaviour
{
    [ContextMenu("Отчистить токены Spotify")]
    public void ClearTokens()
    {
        PlayerPrefs.DeleteKey("access_token");
        PlayerPrefs.DeleteKey("refresh_token");
        PlayerPrefs.DeleteKey("expires_at");
        PlayerPrefs.DeleteKey("PKCE-credentials");
        PlayerPrefs.Save();

    }
}
