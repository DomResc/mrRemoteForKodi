using Windows.Data.Json;

namespace mrRemoteForKodi_Update_1.Helpers
{
    class JsonHelper
    {
        public static string JsonCommandMethod(string method)
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue(method) }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandPing()
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("JSONRPC.Ping") }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandAction(string action)
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("Input.ExecuteAction") },
                { "params", new JsonObject
                                {
                                    { "action", JsonValue.CreateStringValue(action) }
                                }
                }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandWindow(string window)
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("GUI.ActivateWindow") },
                { "params", new JsonObject
                                {
                                    { "window", JsonValue.CreateStringValue(window) }
                                }
                }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandWindowParameters(string window, string parameters)
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("GUI.ActivateWindow") },
                { "params", new JsonObject
                    {
                        { "window", JsonValue.CreateStringValue(window) },
                        { "parameters", new JsonArray
                            {
                                JsonValue.CreateStringValue(parameters)
                            }
                        }
                    }
                }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandPlayerGetItem(int playerId)
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("Player.Getitem") },
                { "params", new JsonObject
                    {
                        { "playerid", JsonValue.CreateNumberValue(playerId) },
                        { "properties", new JsonArray
                            {
                                JsonValue.CreateStringValue("title"),
                                JsonValue.CreateStringValue("albumartist"),
                                JsonValue.CreateStringValue("genre"),
                                JsonValue.CreateStringValue("year"),
                                JsonValue.CreateStringValue("rating"),
                                JsonValue.CreateStringValue("album"),
                                JsonValue.CreateStringValue("track"),
                                JsonValue.CreateStringValue("fanart"),
                                JsonValue.CreateStringValue("director"),
                                JsonValue.CreateStringValue("tagline"),
                                JsonValue.CreateStringValue("plot"),
                                JsonValue.CreateStringValue("writer"),
                                JsonValue.CreateStringValue("firstaired"),
                                JsonValue.CreateStringValue("season"),
                                JsonValue.CreateStringValue("episode"),
                                JsonValue.CreateStringValue("showtitle"),
                                JsonValue.CreateStringValue("thumbnail"),
                                JsonValue.CreateStringValue("art"),
                                JsonValue.CreateStringValue("channel"),
                                JsonValue.CreateStringValue("channelnumber")
                            }
                        }
                    }
                }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandPlayerGetProperties(int playerId)
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("Player.GetProperties") },
                { "params", new JsonObject
                    {
                        { "playerid", JsonValue.CreateNumberValue(playerId) },
                        { "properties", new JsonArray
                            {
                                JsonValue.CreateStringValue("time"),
                                JsonValue.CreateStringValue("totaltime"),
                                JsonValue.CreateStringValue("canseek"),
                                JsonValue.CreateStringValue("percentage"),
                                JsonValue.CreateStringValue("speed"),
                                JsonValue.CreateStringValue("canshuffle"),
                                JsonValue.CreateStringValue("shuffled"),
                                JsonValue.CreateStringValue("canrepeat"),
                                JsonValue.CreateStringValue("repeat"),
                                JsonValue.CreateStringValue("audiostreams"),
                                JsonValue.CreateStringValue("subtitleenabled"),
                                JsonValue.CreateStringValue("subtitles")
                            }
                        }
                    }
                }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandPlayerSetPropertiesSeek(int playerId, int playerSeek)
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("Player.Seek") },
                { "params", new JsonObject
                    {
                        { "playerid", JsonValue.CreateNumberValue(playerId) },
                        { "value", JsonValue.CreateNumberValue(playerSeek) }
                    }
                }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandPlayerSetPropertiesSubtitle(int playerId, int playerSubtitle)
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("Player.SetSubtitle") },
                { "params", new JsonObject
                    {
                        { "playerid", JsonValue.CreateNumberValue(playerId) },
                        { "subtitle", JsonValue.CreateNumberValue(playerSubtitle) }
                    }
                }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandPlayerSetPropertiesAudioStreams(int playerId, int playerSubtitle)
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("Player.SetAudioStream") },
                { "params", new JsonObject
                    {
                        { "playerid", JsonValue.CreateNumberValue(playerId) },
                        { "stream", JsonValue.CreateNumberValue(playerSubtitle) }
                    }
                }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandApplicationGetProperties(string properties)
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("Application.GetProperties") },
                { "params", new JsonObject
                    {
                        { "properties", new JsonArray
                            {
                                JsonValue.CreateStringValue(properties)
                            }
                        }
                    }
                }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandApplicationSetPropertiesVolume(int volume)
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("Application.SetVolume") },
                { "params", new JsonObject
                    {
                        {
                            "volume", JsonValue.CreateNumberValue(volume)
                        }
                    }
                }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandPlayerSetPropertiesRepeat(int playerId, string repeat)
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("Player.SetRepeat") },
                { "params", new JsonObject
                    {
                        { "playerid", JsonValue.CreateNumberValue(playerId) },
                        { "repeat", JsonValue.CreateStringValue(repeat) }
                    }
                }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandPlayerSetPropertiesShuffle(int playerId, bool shuffle)
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("Player.SetShuffle") },
                { "params", new JsonObject
                    {
                        { "playerid", JsonValue.CreateNumberValue(playerId) },
                        { "shuffle", JsonValue.CreateBooleanValue(shuffle) }
                    }
                }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandInputSendText(string text)
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("Input.SendText") },
                { "params", new JsonObject
                                {
                                    { "text", JsonValue.CreateStringValue(text) },
                                    { "done", JsonValue.CreateBooleanValue(true)}
                                }
                }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandVideoLibraryGetMovies()
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("VideoLibrary.GetMovies") },
                { "params", new JsonObject
                    {
                        { "properties", new JsonArray
                            {
                                JsonValue.CreateStringValue("title"),
                                JsonValue.CreateStringValue("genre"),
                                JsonValue.CreateStringValue("year"),
                                JsonValue.CreateStringValue("rating"),
                                JsonValue.CreateStringValue("director"),
                                JsonValue.CreateStringValue("tagline"),
                                JsonValue.CreateStringValue("plot"),
                                JsonValue.CreateStringValue("writer"),
                                JsonValue.CreateStringValue("runtime"),
                                JsonValue.CreateStringValue("dateadded"),
                                JsonValue.CreateStringValue("imdbnumber"),
                                JsonValue.CreateStringValue("file"),
                                JsonValue.CreateStringValue("art")
                            }
                        }
                    }
                }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandVideoLibraryGetTVshows()
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("VideoLibrary.GetTVShows") },
                { "params", new JsonObject
                    {
                        { "properties", new JsonArray
                            {
                                JsonValue.CreateStringValue("title"),
                                JsonValue.CreateStringValue("genre"),
                                JsonValue.CreateStringValue("year"),
                                JsonValue.CreateStringValue("rating"),
                                JsonValue.CreateStringValue("plot"),
                                JsonValue.CreateStringValue("episode"),
                                JsonValue.CreateStringValue("season"),
                                JsonValue.CreateStringValue("dateadded"),
                                JsonValue.CreateStringValue("art")
                            }
                        }
                    }
                }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandPlayerOpen(string library, int id)
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("Player.Open") },
                { "params", new JsonObject
                    {
                        { "item", new JsonObject
                            {
                                { library, JsonValue.CreateNumberValue(id) }
                            }
                        }
                    }
                }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandPlayerOpenFile(string file)
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("Player.Open") },
                { "params", new JsonObject
                    {
                        { "item", new JsonObject
                            {
                                { "file", JsonValue.CreateStringValue(file) }
                            }
                        }
                    }
                }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandVideoLibraryGetSeasons(int id)
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("VideoLibrary.GetSeasons") },
                { "params", new JsonObject
                    {
                        { "tvshowid", JsonValue.CreateNumberValue(id) },
                        { "properties", new JsonArray
                            {
                                JsonValue.CreateStringValue("season"),
                                JsonValue.CreateStringValue("thumbnail")
                            }
                        }
                    }
                }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandVideoLibraryGetEpisodes(int id, int season)
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("VideoLibrary.GetEpisodes") },
                { "params", new JsonObject
                    {
                        { "tvshowid", JsonValue.CreateNumberValue(id) },
                        { "season", JsonValue.CreateNumberValue(season) },
                        { "properties", new JsonArray
                            {
                                JsonValue.CreateStringValue("plot"),
                                JsonValue.CreateStringValue("playcount"),
                                JsonValue.CreateStringValue("file")
                            }
                        }
                    }
                }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandPvrGetChannels(string channeltype)
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("PVR.GetChannels") },
                { "params", new JsonObject
                    {
                        { "channelgroupid", JsonValue.CreateStringValue(channeltype) },
                    }
                }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandAudioLibraryGetArtist()
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("AudioLibrary.GetArtists") },
                { "params", new JsonObject
                    {
                        { "properties", new JsonArray
                            {
                                JsonValue.CreateStringValue("description"),
                                JsonValue.CreateStringValue("genre"),
                                JsonValue.CreateStringValue("fanart"),
                                JsonValue.CreateStringValue("thumbnail")
                            }
                        }
                    }
                }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandAudioLibraryGetAlbum(int id)
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("AudioLibrary.GetAlbums") },
                { "params", new JsonObject
                    {
                        {"filter", new JsonObject
                            {
                                { "artistid", JsonValue.CreateNumberValue(id) }
                            }
                        },
                        { "properties", new JsonArray
                            {
                                JsonValue.CreateStringValue("thumbnail")
                            }
                        }
                    }
                }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandAudioLibraryGetSong(int albumid)
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("AudioLibrary.GetSongs") },
                { "params", new JsonObject
                    {
                        { "filter", new JsonObject
                            {
                                { "albumid", JsonValue.CreateNumberValue(albumid) }
                            }
                        },
                        { "properties", new JsonArray
                            {
                                JsonValue.CreateStringValue("track")
                            }
                        }
                    }
                }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandFilesGetSources(string mediaType)
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("Files.GetSources") },
                { "params", new JsonObject
                    {
                        { "media", JsonValue.CreateStringValue(mediaType) },
                    }
                }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandFilesGetDirectory(string directory)
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("Files.GetDirectory") },
                { "params", new JsonObject
                    {
                        { "directory", JsonValue.CreateStringValue(directory) },
                    }
                }
            };
            return jsonObject.Stringify();
        }

        public static string JsonCommandFilesDownload(string path)
        {
            JsonObject jsonObject = new JsonObject
            {
                { "id", JsonValue.CreateNumberValue(0) },
                { "jsonrpc", JsonValue.CreateStringValue("2.0") },
                { "method", JsonValue.CreateStringValue("Files.PrepareDownload") },
                { "params", new JsonObject
                    {
                        { "path", JsonValue.CreateStringValue(path) },
                    }
                }
            };
            return jsonObject.Stringify();
        }
    }
}