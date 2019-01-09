using mrRemoteForKodi_Update_1.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace mrRemoteForKodi_Update_1.Helpers
{
    public class GetInformationHelper
    {
        // return first playerid from the response else return -1
        public static int getActivePlayersId(string response)
        {
            try
            {
                var activePlayer = JObject.Parse(response);

                if (activePlayer["result"].HasValues)
                {
                    return (int)activePlayer["result"].First["playerid"];
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return -1;
            }
        }

        // return fanart or null
        public static string getPlayerItemFanart(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (string.IsNullOrWhiteSpace((string)player["result"]["item"]["art"]["fanart"]) != true)
                {
                    return (string)player["result"]["item"]["art"]["fanart"];
                }
                else if (string.IsNullOrWhiteSpace((string)player["result"]["item"]["art"]["tvshow.fanart"]) != true)
                {
                    return (string)player["result"]["item"]["art"]["tvshow.fanart"];
                }
                else if (string.IsNullOrWhiteSpace((string)player["result"]["item"]["fanart"]) != true)
                {
                    return (string)player["result"]["item"]["fanart"];
                }
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return thumb or null
        public static string getPlayerItemThumb(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (string.IsNullOrWhiteSpace((string)player["result"]["item"]["art"]["poster"]) != true)
                {
                    return (string)player["result"]["item"]["art"]["poster"];
                }
                else if (string.IsNullOrWhiteSpace((string)player["result"]["item"]["art"]["season.poster"]) != true)
                {
                    return (string)player["result"]["item"]["art"]["season.poster"];
                }
                else if (string.IsNullOrWhiteSpace((string)player["result"]["item"]["art"]["tvshow.poster"]) != true)
                {
                    return (string)player["result"]["item"]["art"]["tvshow.poster"];
                }
                else if (string.IsNullOrWhiteSpace((string)player["result"]["item"]["thumbnail"]) != true)
                {
                    return (string)player["result"]["item"]["thumbnail"];
                }
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return title or null
        public static string getPlayerItemTitle(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (string.IsNullOrWhiteSpace((string)player["result"]["item"]["title"]) != true)
                {
                    return (string)player["result"]["item"]["title"];
                }
                if (string.IsNullOrWhiteSpace((string)player["result"]["item"]["label"]) != true)
                {
                    return (string)player["result"]["item"]["label"];
                }
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return year or null
        public static string getPlayerItemYear(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if ((string.IsNullOrWhiteSpace((string)player["result"]["item"]["year"]) != true) && ((string)player["result"]["item"]["year"] != "0"))
                {
                    return (string)player["result"]["item"]["year"];
                }
                else if (string.IsNullOrWhiteSpace((string)player["result"]["item"]["firstaired"]) != true)
                {
                    return (string)player["result"]["item"]["firstaired"];
                }
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return rating or null
        public static string getPlayerItemRating(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if ((string.IsNullOrWhiteSpace((string)player["result"]["item"]["rating"]) != true) && ((string)player["result"]["item"]["rating"] != "0"))
                {
                    return (string)player["result"]["item"]["rating"];
                }
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return genre or null
        public static string getPlayerItemGenre(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (player["result"]["item"]["genre"] != null)
                    if (player["result"]["item"]["genre"].HasValues)
                    {
                        return string.Join(", ", (player["result"]["item"]["genre"]));
                    }
                    else return null;
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return tagline or null
        public static string getPlayerItemTagline(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (string.IsNullOrWhiteSpace((string)player["result"]["item"]["tagline"]) != true)
                {
                    return (string)player["result"]["item"]["tagline"];
                }
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return showtitle or null
        public static string getPlayerItemShowTitle(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (string.IsNullOrWhiteSpace((string)player["result"]["item"]["showtitle"]) != true)
                {
                    return (string)player["result"]["item"]["showtitle"];
                }
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return season or null
        public static string getPlayerItemSeason(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if ((string.IsNullOrWhiteSpace((string)player["result"]["item"]["season"]) != true) && ((string)player["result"]["item"]["season"] != "-1"))
                {
                    return (string)player["result"]["item"]["season"];
                }
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return episode or null
        public static string getPlayerItemEpisode(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if ((string.IsNullOrWhiteSpace((string)player["result"]["item"]["episode"]) != true) && ((string)player["result"]["item"]["episode"] != "-1"))
                {
                    return (string)player["result"]["item"]["episode"];
                }
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return albumartist or null
        public static string getPlayerItemAlbumArtist(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (player["result"]["item"]["albumartist"] != null)
                    if (player["result"]["item"]["albumartist"].HasValues)
                    {
                        return string.Join(", ", (player["result"]["item"]["albumartist"]));
                    }
                    else return null;
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return the album or null
        public static string getPlayerItemAlbum(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (string.IsNullOrWhiteSpace((string)player["result"]["item"]["album"]) != true)
                {
                    return (string)player["result"]["item"]["album"];
                }
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return the track or null
        public static string getPlayerItemTrack(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if ((string.IsNullOrWhiteSpace((string)player["result"]["item"]["track"]) != true) && ((string)player["result"]["item"]["track"] != "-1"))
                {
                    return (string)player["result"]["item"]["track"];
                }
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return channel or null
        public static string getPlayerItemChannel(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (string.IsNullOrWhiteSpace((string)player["result"]["item"]["channel"]) != true)
                {
                    return (string)player["result"]["item"]["channel"];
                }
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return channelnumber or null
        public static string getPlayerItemChannelNumber(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (string.IsNullOrWhiteSpace((string)player["result"]["item"]["channelnumber"]) != true)
                {
                    return (string)player["result"]["item"]["channelnumber"];
                }
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return director or null
        public static string getPlayerItemDirector(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (player["result"]["item"]["director"] != null)
                    if (player["result"]["item"]["director"].HasValues)
                    {
                        return string.Join(", ", (player["result"]["item"]["director"]));
                    }
                    else return null;
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return writer or null
        public static string getPlayerItemWriter(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (player["result"]["item"]["writer"] != null)
                    if (player["result"]["item"]["writer"].HasValues)
                    {
                        return string.Join(", ", (player["result"]["item"]["writer"]));
                    }
                    else return null;
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return plot or null
        public static string getPlayerItemPlot(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (string.IsNullOrWhiteSpace((string)player["result"]["item"]["plot"]) != true)
                {
                    return (string)player["result"]["item"]["plot"];
                }
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return canseek or null
        public static string getPlayerPropertiesCanSeek(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (string.IsNullOrWhiteSpace((string)player["result"]["canseek"]) != true)
                {
                    return (string)player["result"]["canseek"];
                }
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return percentage or null
        public static string getPlayerPropertiesPercentage(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (string.IsNullOrWhiteSpace((string)player["result"]["percentage"]) != true)
                {
                    return Convert.ToString((int)player["result"]["percentage"]);
                }
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return time or null
        public static string getPlayerPropertiesTime(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (player["result"]["time"] != null)
                {
                    var timeSpan = new TimeSpan((Int32)player["result"]["time"]["hours"], (Int32)player["result"]["time"]["minutes"], (Int32)player["result"]["time"]["seconds"]);

                    return timeSpan.ToString(@"hh\:mm\:ss");
                }
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return totaltime or null
        public static string getPlayerPropertiesTotalTime(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (player["result"]["totaltime"] != null)
                {
                    var timeSpan = new TimeSpan((Int32)player["result"]["totaltime"]["hours"], (Int32)player["result"]["totaltime"]["minutes"], (Int32)player["result"]["totaltime"]["seconds"]);

                    return timeSpan.ToString(@"hh\:mm\:ss");
                }
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return speed or null
        public static string getPlayerPropertiesSpeed(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (string.IsNullOrWhiteSpace((string)player["result"]["speed"]) != true)
                {
                    return Convert.ToString((int)player["result"]["speed"]);
                }
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return subtitles list or null
        public static List<Subtitle> getPlayerPropertiesSubtitles(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (player["result"]["subtitles"] != null)
                    if (player["result"]["subtitles"].HasValues)
                    {
                        var subtitleArray = player["result"]["subtitles"];

                        var subtitleList = subtitleArray.Select(p => new Subtitle
                        {
                            Index = (int)p["index"],
                            Language = (string)p["language"],
                            Name = (string)p["name"]
                        }).ToList();

                        return subtitleList;
                    }
                    else return null;
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return the current subtitle
        public static string getPlayerPropertiesSubtitleEnabled(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (string.IsNullOrWhiteSpace((string)player["result"]["subtitleenabled"]) != true)
                {
                    return (string)player["result"]["subtitleenabled"];
                }
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return audiostreams list or null
        public static List<AudioStream> getPlayerPropertiesAudiostreams(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (player["result"]["audiostreams"] != null)
                    if (player["result"]["audiostreams"].HasValues)
                    {
                        var audiostreamsArray = player["result"]["audiostreams"];

                        var audiostreamsList = audiostreamsArray.Select(p => new AudioStream
                        {
                            Index = (int)p["index"],
                            Language = (string)p["language"],
                            Name = (string)p["name"]
                        }).ToList();

                        return audiostreamsList;
                    }
                    else return null;
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return the current volume
        public static string getApplicationPropertiesVolume(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (string.IsNullOrWhiteSpace((string)player["result"]["volume"]) != true)
                {
                    return (string)player["result"]["volume"];
                }
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return speed or null
        public static string getPlayerPropertiesCanRepeat(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (string.IsNullOrWhiteSpace((string)player["result"]["canrepeat"]) != true)
                {
                    return (string)player["result"]["canrepeat"];
                }
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return speed or null
        public static string getPlayerPropertiesCanShuffle(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (string.IsNullOrWhiteSpace((string)player["result"]["canshuffle"]) != true)
                {
                    return (string)player["result"]["canshuffle"];
                }
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return movie list or null
        public static List<Movie> getVideoLibrary(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (player["result"]["movies"] != null)
                    if (player["result"]["movies"].HasValues)
                    {
                        var movieArray = player["result"]["movies"];

                        var movieList = movieArray.Select(p => new Movie
                        {
                            Title = (string)p["title"],
                            Genre = string.Join(", ", p["genre"]),
                            Year = (string)p["year"],
                            Rating = (string)p["rating"],
                            Director = string.Join(", ", p["director"]),
                            Tagline = (string)p["tagline"],
                            Plot = (string)p["plot"],
                            Writer = string.Join(", ", p["writer"]),
                            Runtime = (string)p["runtime"],
                            DateAdded = (string)p["dateadded"],
                            ImdbNumber = (string)p["imdbnumber"],
                            Fanart = (string)p["art"]["fanart"],
                            Poster = (string)p["art"]["poster"],
                            File = (string)p["file"],
                            Movieid = (string)p["movieid"]
                        }).ToList();

                        return movieList;
                    }
                    else return null;
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return movie list or null
        public static List<TvShow> getTvShowLibrary(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (player["result"]["tvshows"] != null)
                    if (player["result"]["tvshows"].HasValues)
                    {
                        var tvShowArray = player["result"]["tvshows"];

                        var tvShowList = tvShowArray.Select(p => new TvShow
                        {
                            Title = (string)p["title"],
                            Genre = string.Join(", ", p["genre"]),
                            Year = (string)p["year"],
                            Rating = (string)p["rating"],
                            Plot = (string)p["plot"],
                            Episode = (string)p["episode"],
                            Season = (string)p["season"],
                            DateAdded = (string)p["dateadded"],
                            Fanart = (string)p["art"]["fanart"],
                            Poster = (string)p["art"]["poster"],
                            Tvshowid = (string)p["tvshowid"]
                        }).ToList();

                        return tvShowList;
                    }
                    else return null;
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return season list or null
        public static List<Season> getSeasonLibrary(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (player["result"]["seasons"] != null)
                    if (player["result"]["seasons"].HasValues)
                    {
                        var seasonArray = player["result"]["seasons"];

                        var seasonList = seasonArray.Select(p => new Season
                        {
                            Label = (string)p["label"],
                            SeasonNumber = (string)p["season"],
                            Thumb = (string)p["thumbnail"]
                        }).ToList();

                        return seasonList;
                    }
                    else return null;
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return episode list or null
        public static List<Episode> getEpisodeLibrary(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (player["result"]["episodes"] != null)
                    if (player["result"]["episodes"].HasValues)
                    {
                        var episodeArray = player["result"]["episodes"];

                        var episodeList = episodeArray.Select(p => new Episode
                        {
                            Label = (string)p["label"],
                            EpisodeId = (string)p["episodeid"],
                            Plot = (string)p["plot"],
                            Playcount = (string)p["playcount"],
                            File = (string)p["file"]
                        }).ToList();

                        return episodeList;
                    }
                    else return null;
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return channel list or null
        public static List<PvrChannel> getPvrChannel(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (player["result"]["channels"] != null)
                    if (player["result"]["channels"].HasValues)
                    {
                        var channelArray = player["result"]["channels"];

                        var channelList = channelArray.Select(p => new PvrChannel
                        {
                            Label = (string)p["label"],
                            ChannelId = (string)p["channelid"]
                        }).ToList();

                        return channelList;
                    }
                    else return null;
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return artist list or null
        public static List<Artist> getAudioLibraryArtist(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (player["result"]["artists"] != null)
                    if (player["result"]["artists"].HasValues)
                    {
                        var artistArray = player["result"]["artists"];

                        var artistList = artistArray.Select(p => new Artist
                        {
                            Label = (string)p["label"],
                            ArtistId = (string)p["artistid"],
                            Genre = string.Join(", ", p["genre"]),
                            Description = (string)p["description"],
                            Fanart = (string)p["fanart"],
                            Poster = (string)p["thumbnail"]
                        }).ToList();

                        return artistList;
                    }
                    else return null;
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return album list or null
        public static List<Album> getAudioLibraryAlbum(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (player["result"]["albums"] != null)
                    if (player["result"]["albums"].HasValues)
                    {
                        var albumArray = player["result"]["albums"];

                        var albumList = albumArray.Select(p => new Album
                        {
                            Label = (string)p["label"],
                            AlbumId = (string)p["albumid"],
                            Poster = (string)p["thumbnail"]
                        }).ToList();

                        return albumList;
                    }
                    else return null;
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return song list or null
        public static List<Song> getAudioLibrarySong(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (player["result"]["songs"] != null)
                    if (player["result"]["songs"].HasValues)
                    {
                        var songArray = player["result"]["songs"];

                        var songList = songArray.Select(p => new Song
                        {
                            Label = (string)p["label"],
                            SongId = (string)p["songid"],
                            Track = (string)p["track"]
                        }).ToList();

                        return songList;
                    }
                    else return null;
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return song list or null
        public static List<FileDirectory> getDirectoryFile(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (player["result"]["sources"] != null)
                    if (player["result"]["sources"].HasValues)
                    {
                        var fileArray = player["result"]["sources"];

                        var fileList = fileArray.Select(p => new FileDirectory
                        {
                            Label = (string)p["label"],
                            Directory = (string)p["file"]
                        }).ToList();

                        return fileList;
                    }
                    else return null;
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return song list or null
        public static List<FileMedia> getFile(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (player["result"]["files"] != null)
                    if (player["result"]["files"].HasValues)
                    {
                        var fileArray = player["result"]["files"];

                        var fileList = fileArray.Select(p => new FileMedia
                        {
                            Label = (string)p["label"],
                            Directory = (string)p["file"],
                            Filetype = (string)p["filetype"]
                        }).ToList();

                        return fileList;
                    }
                    else return null;
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        // return file or null
        public static string getVideoFile(string response)
        {
            try
            {
                var player = JObject.Parse(response);

                if (string.IsNullOrWhiteSpace((string)player["result"]["details"]["path"]) != true)
                {
                    return (string)player["result"]["details"]["path"];
                }
                else return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}