using System.Runtime.Serialization;

namespace Api.Soap.Models;

[DataContract]
public class UserSoapDto
{
  [DataMember]
  public int Id { get; set; }

  [DataMember]
  public string Name { get; set; } = string.Empty;

  [DataMember]
  public DateTime BirthDate { get; set; }

  [DataMember]
  public string Email { get; set; } = string.Empty;

  [DataMember]
  public int Age { get; set; }
}

[DataContract]
public class CreateUserRequest
{
  [DataMember]
  public string Name { get; set; } = string.Empty;

  [DataMember]
  public DateTime BirthDate { get; set; }

  [DataMember]
  public string Email { get; set; } = string.Empty;
}

[DataContract]
public class MusicSoapDto
{
  [DataMember]
  public int Id { get; set; }

  [DataMember]
  public string Name { get; set; } = string.Empty;

  [DataMember]
  public string Artist { get; set; } = string.Empty;

  [DataMember]
  public string AudioUrl { get; set; } = string.Empty;
}

[DataContract]
public class CreateMusicRequest
{
  [DataMember]
  public string Name { get; set; } = string.Empty;

  [DataMember]
  public string Artist { get; set; } = string.Empty;

  [DataMember]
  public string AudioUrl { get; set; } = string.Empty;
}

[DataContract]
public class PlaylistSoapDto
{
  [DataMember]
  public int Id { get; set; }

  [DataMember]
  public string Name { get; set; } = string.Empty;

  [DataMember]
  public int? UserId { get; set; }

  [DataMember]
  public bool IsSystemPlaylist { get; set; }

  [DataMember]
  public DateTime CreatedAt { get; set; }

  [DataMember]
  public string? OwnerName { get; set; }
}

[DataContract]
public class CreatePlaylistRequest
{
  [DataMember]
  public string Name { get; set; } = string.Empty;

  [DataMember]
  public int? UserId { get; set; }

  [DataMember]
  public List<int>? MusicIds { get; set; }
}

[DataContract]
public class GetUsersRequest
{
  [DataMember]
  public int? Page { get; set; }

  [DataMember]
  public int? PageSize { get; set; }
}

[DataContract]
public class GetUsersResponse
{
  [DataMember]
  public List<UserSoapDto> Users { get; set; } = new();

  [DataMember]
  public int TotalCount { get; set; }

  [DataMember]
  public int? Page { get; set; }

  [DataMember]
  public int? PageSize { get; set; }

  [DataMember]
  public int? TotalPages { get; set; }
}

[DataContract]
public class GetMusicsRequest
{
  [DataMember]
  public int? PlaylistId { get; set; }

  [DataMember]
  public string? Artist { get; set; }

  [DataMember]
  public string? SearchTerm { get; set; }

  [DataMember]
  public int? Page { get; set; }

  [DataMember]
  public int? PageSize { get; set; }
}

[DataContract]
public class GetMusicsResponse
{
  [DataMember]
  public List<MusicSoapDto> Musics { get; set; } = new();

  [DataMember]
  public int TotalCount { get; set; }

  [DataMember]
  public int? Page { get; set; }

  [DataMember]
  public int? PageSize { get; set; }

  [DataMember]
  public int? TotalPages { get; set; }
}

[DataContract]
public class GetPlaylistsRequest
{
  [DataMember]
  public int? UserId { get; set; }

  [DataMember]
  public bool? SystemOnly { get; set; }

  [DataMember]
  public int? Page { get; set; }

  [DataMember]
  public int? PageSize { get; set; }
}

[DataContract]
public class GetPlaylistsResponse
{
  [DataMember]
  public List<PlaylistSoapDto> Playlists { get; set; } = new();

  [DataMember]
  public int TotalCount { get; set; }

  [DataMember]
  public int? Page { get; set; }

  [DataMember]
  public int? PageSize { get; set; }

  [DataMember]
  public int? TotalPages { get; set; }
}

[DataContract]
public class SoapFaultResponse
{
  [DataMember]
  public string ErrorMessage { get; set; } = string.Empty;

  [DataMember]
  public string ErrorType { get; set; } = string.Empty;
}
