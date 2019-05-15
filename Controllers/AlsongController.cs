using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace lyrics_api.Controllers {
  [Route ("api")]
  [EnableCors ("ALLOW_ANY")]
  [Authorize]
  [ApiController]
  public class ValuesController : ControllerBase {
    // GET api/values

    // GET api/values
    [HttpGet]
    [Route ("lyrics")]
    public async Task<ActionResult> Get (string title, string artist) {
      var url = "http://lyrics.alsong.co.kr/alsongwebservice/service1.asmx";
      var param = String.Format (@"<?xml version=""1.0"" encoding=""UTF-8""?><SOAP-ENV:Envelope 
            xmlns:SOAP-ENV=""http://www.w3.org/2003/05/soap-envelope"" xmlns:SOAP-ENC=""http://www.w3.org/2003/05/soap-encoding"" 
            xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
            xmlns:ns2=""ALSongWebServer/Service1Soap"" xmlns:ns1=""ALSongWebServer"" 
            xmlns:ns3=""ALSongWebServer/Service1Soap12"">
            <SOAP-ENV:Body>
            <ns1:GetResembleLyric2>
            <ns1:stQuery>
            <ns1:strTitle>{0}</ns1:strTitle>
            <ns1:strArtistName>{1}</ns1:strArtistName>
            <ns1:nCurPage>0</ns1:nCurPage>
            </ns1:stQuery></ns1:GetResembleLyric2></SOAP-ENV:Body>
            </SOAP-ENV:Envelope>", title, artist);
      string resultContent = null;
      XmlDocument xdoc = new XmlDocument ();
      XmlNodeList xnode = null;
      // Uri proxyUri = new Uri("http://127.0.0.1:1083");
      // var proxy = new CoreWebProxy(proxyUri);
      // HttpClientHandler httpClientHandler = new HttpClientHandler(){
      //     Proxy = proxy,
      // };
      using (var httpClient = new HttpClient ()) { //httpClientHandler)){
        //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml;charset=utf-8"));
        //httpClient.DefaultRequestHeaders.Accept.Add()
        var content = new StringContent (param, System.Text.Encoding.UTF8, "text/xml");
        using (var response = await httpClient.PostAsync (url, content)) {
          resultContent = await response.Content.ReadAsStringAsync ();
          xdoc.LoadXml (resultContent);
          xnode = xdoc.GetElementsByTagName ("GetResembleLyric2Result");
        }

      }
      return Ok (xnode[0]);
      // var client = new RestClient("http://lyrics.alsong.co.kr/alsongwebservice/service1.asmx");
      // var request = new RestRequest(Method.POST);
      // request.AddHeader("postman-token", "baa1ac9c-344d-33f3-0e1f-4ce011f82712");
      // request.AddHeader("cache-control", "no-cache");
      // request.AddHeader("content-type", "text/xml;charset=utf8");
      // request.AddParameter("text/xml;charset=utf8", "<?xml version=\"1.0\" encoding=\"UTF-8\"?><SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:SOAP-ENC=\"http://www.w3.org/2003/05/soap-encoding\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:ns2=\"ALSongWebServer/Service1Soap\" xmlns:ns1=\"ALSongWebServer\" xmlns:ns3=\"ALSongWebServer/Service1Soap12\"><SOAP-ENV:Body><ns1:GetResembleLyric2><ns1:stQuery><ns1:strTitle>아마도 그건 </ns1:strTitle><ns1:strArtistName>홍민정</ns1:strArtistName><ns1:nCurPage>0</ns1:nCurPage></ns1:stQuery></ns1:GetResembleLyric2></SOAP-ENV:Body></SOAP-ENV:Envelope>", ParameterType.RequestBody);
      // IRestResponse response = client.Execute(request);

    }
  }
}