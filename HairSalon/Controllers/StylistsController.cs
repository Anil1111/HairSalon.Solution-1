using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using HairSalon.Models;

namespace HairSalon.Controllers
{
  public class StylistsController : Controller
  {
    [HttpGet("/stylists")]
    public ActionResult Index() {
      List<StylistClass> allStylists = StylistClass.GetAll();
      return View(allStylists);
    }

    [HttpPost("/stylists/delete")]
    public ActionResult DeleteAll()
    {
      StylistClass.ClearAll();
      return View();
    }

    [HttpPost("stylists/{stylistId}/delete")]
    public ActionResult Destroy(int stylistId){
      StylistClass.Find(stylistId).Delete();
      return RedirectToAction("Index");
    }

    [HttpPost("/stylists/{stylistId}/clients/{clientId}/delete")]
    public ActionResult Destroy(int stylistId, int clientId){
      ClientClass.Find(clientId).Delete();
      StylistClass foundStylist = StylistClass.Find(stylistId);
      List<ClientClass> stylistClients = foundStylist.GetClients();
      Dictionary<string, object> model = new Dictionary<string, object>();
      model.Add("stylist", foundStylist);
      model.Add("clients", stylistClients);
      return View("Show", model);
    }

    [HttpGet("/stylists/new")]
    public ActionResult New() { return View(); }

    [HttpPost("/stylists")]
    public ActionResult Create(string name, string phoneNumber){
      StylistClass stylist = new StylistClass(name, phoneNumber);
      stylist.Save();
      return RedirectToAction("Index");
    }

    [HttpGet("/stylists/{id}")]
    public ActionResult Show(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      StylistClass foundStylist = StylistClass.Find(id);
      List<ClientClass> stylistClients = foundStylist.GetClients();
      model.Add("stylist", foundStylist);
      model.Add("clients", stylistClients);
      return View(model);
    }

    [HttpPost("/stylists/{stylistId}/clients")]
    public ActionResult Create(int stylistId, string name, string phoneNumber)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      StylistClass foundStylist = StylistClass.Find(stylistId);
      ClientClass newClient = new ClientClass(name, phoneNumber, stylistId);
      newClient.Save();
      List<ClientClass> stylistClients = foundStylist.GetClients();
      model.Add("stylist", foundStylist);
      model.Add("clients", stylistClients);
      return View("Show", model);
    }

    [HttpGet("stylists/{stylistId}/edit")]
    public ActionResult Edit(int stylistId)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      StylistClass stylist = StylistClass.Find(stylistId);
      return View(stylist);
    }

    [HttpPost("/stylists/{stylistId}")]
    public ActionResult Update(int stylistId, string name, string phoneNumber)
    {
      StylistClass stylist = StylistClass.Find(stylistId);
      stylist.Edit(name, phoneNumber);
      Dictionary<string, object> model = new Dictionary<string, object>();
      List<ClientClass> stylistClients = stylist.GetClients();
      model.Add("stylist", stylist);
      model.Add("clients", stylistClients);
      return View("Show", model);
    }
  }
}
