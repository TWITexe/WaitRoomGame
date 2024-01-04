using SpotifyAPI.Web;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

/// <summary>
/// Spotify Player Controller script. Script to emulate the bottom player bar in Spotify
/// This is an example script of how/what you could/should implement
/// </summary>
public class SpotifyPlayerController : SpotifyPlayerListener
{
    // Main track image
    [SerializeField]
    private Image _trackIcon;
    // Current track name & artist text
    [SerializeField]
    private Text _trackName, _artistsNames;
    // Button to add/remove track to user's library
    
    // Player middle track progress left & right text
    [SerializeField]
    private Text _currentProgressText, _totalProgressText;
    [SerializeField]
    private Sprite _playSprite, _pauseSprite, _muteSprite, _unmuteSprite;

    // Is the current track in the user's library?
    private bool _currentItemIsInLibrary;
    // Did the user mouse down on the progress slider to edit the progress
    private bool _progressStartDrag = false;
    // Current progress value when user is sliding the progress
    private float _progressDragNewValue = -1.0f;


    protected override void Awake()
    {
        base.Awake();

        // Listen to needed events on Awake
        this.OnPlayingItemChanged += this.PlayingItemChanged;
    }
    

    private void Update()
    {
        CurrentlyPlayingContext context = GetCurrentContext();
        if (context != null)
        {
            // Update current position to context position when user is not dragging
            if (_currentProgressText != null && !_progressStartDrag)
            {
                _currentProgressText.text = S4UUtility.MsToTimeString(context.ProgressMs);
            }

            FullTrack track = context.Item as FullTrack;
            if (track != null)
            {
                if (_totalProgressText != null)
                {
                    _totalProgressText.text = S4UUtility.MsToTimeString(track.DurationMs);
                }
            }
        }
    }

    private async void PlayingItemChanged(IPlayableItem newPlayingItem)
    {
        if (newPlayingItem == null)
        {
            // No new item playing, reset UI
            UpdatePlayerInfo("No track playing", "No track playing", "");
            _totalProgressText.text = _currentProgressText.text = "00:00";
        }
        else
        {
            if (newPlayingItem.Type == ItemType.Track)
            {
                if (newPlayingItem is FullTrack track)
                {
                    // Update player information with track info
                    string allArtists = S4UUtility.ArtistsToSeparatedString(", ", track.Artists);
                    SpotifyAPI.Web.Image image = S4UUtility.GetLowestResolutionImage(track.Album.Images);
                    UpdatePlayerInfo(track.Name, allArtists, image?.Url);

                    // Make request to see if track is part of user's library
                    var client = SpotifyService.Instance.GetSpotifyClient();
                    LibraryCheckTracksRequest request = new LibraryCheckTracksRequest(new List<string>() { track.Id });
                    var result = await client.Library.CheckTracks(request);
                }
            }
            else if (newPlayingItem.Type == ItemType.Episode)
            {
                if (newPlayingItem is FullEpisode episode)
                {
                    string creators = episode.Show.Publisher;
                    SpotifyAPI.Web.Image image = S4UUtility.GetLowestResolutionImage(episode.Images);
                    UpdatePlayerInfo(episode.Name, creators, image?.Url);
                }
            }
        }
    }

    // Updates the left hand side of the player (Artwork, track name, artists)
    private void UpdatePlayerInfo(string trackName, string artistNames, string artUrl)
    {
        if (_trackName != null)
        {
            _trackName.text = trackName;
        }
        if (_artistsNames != null)
        {
            _artistsNames.text = artistNames;
        }
        if (_trackIcon != null)
        {
            // Load sprite from url
            if (string.IsNullOrEmpty(artUrl))
            {
                _trackIcon.sprite = null;
            }
            else
            {
                StartCoroutine(S4UUtility.LoadImageFromUrl(artUrl, (loadedSprite) =>
                {
                    _trackIcon.sprite = loadedSprite;
                }));
            }
        }
    }
}
