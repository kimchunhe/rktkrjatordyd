using System;
using System.Net;

namespace aspnetapp.Utils {
  public class CoreWebProxy : IWebProxy {
    public readonly Uri Uri;
    private readonly bool bypass;

    public CoreWebProxy (Uri uri, ICredentials credentials = null, bool bypass = false) {
      Uri = uri;
      this.bypass = bypass;
      Credentials = credentials;
    }

    public ICredentials Credentials { get; set; }

    public Uri GetProxy (Uri destination) => Uri;

    public bool IsBypassed (Uri host) => bypass;

    public override int GetHashCode () {
      if (Uri == null) {
        return -1;
      }

      return Uri.GetHashCode ();
    }
  }
}