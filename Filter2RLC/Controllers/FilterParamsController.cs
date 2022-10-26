using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Filter2RLC.Models;
using System.Numerics;
using System.IO;
using System.Web.UI.DataVisualization.Charting;

namespace Filter2RLC.Controllers
{
    public class FilterParamsController : Controller
    {
        private FilterDBContext db = new FilterDBContext();

        // GET: FilterParams
        public ActionResult Index()
        {
            return View(db.SetOfFilters.ToList());
        }

        // GET: FilterParams/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilterParams filterParams = db.SetOfFilters.Find(id);
            if (filterParams == null)
            {
                return HttpNotFound();
            }
            return View(filterParams);
        }

        // GET: FilterParams/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FilterParams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Resistance1,Resistance2,Inductance,Capacitance,U1,Fmin,Fmax,NumOfRows")] FilterParams filterParams)
        {
            if (ModelState.IsValid)
            {
                db.SetOfFilters.Add(filterParams);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(filterParams);
        }
        
        // GET: FilterParams/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilterParams filterParams = db.SetOfFilters.Find(id);
            if (filterParams == null)
            {
                return HttpNotFound();
            }
            return View(filterParams);
        }

        // POST: FilterParams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Resistance1,Resistance2,Inductance,Capacitance,U1,Fmin,Fmax,NumOfRows")] FilterParams filterParams)
        {
            if (ModelState.IsValid)
            {
                db.Entry(filterParams).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(filterParams);
        }
        // GET: FilterParams/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilterParams filterParams = db.SetOfFilters.Find(id);
            if (filterParams == null)
            {
                return HttpNotFound();
            }
            return View(filterParams);
        }

        // POST: FilterParams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FilterParams filterParams = db.SetOfFilters.Find(id);
            db.SetOfFilters.Remove(filterParams);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Helpers()
        {
            FilterParams filterParams = db.SetOfFilters.FirstOrDefault();
            if (filterParams == null)
            {
                return HttpNotFound();
            }
            return View(filterParams);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        //---Added waveform
        public ActionResult CreateWaveform(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FilterParams filter = db.SetOfFilters.Find(id);
            if (filter == null)
            {
                return HttpNotFound();
            }
            //---
            DataTable dTable;
            DataView dView;
            dTable = new DataTable();
            DataColumn column;
            DataRow row;
            //---
            column = new DataColumn();
            column.DataType = Type.GetType("System.Double");
            column.ColumnName = "Frequency";
            dTable.Columns.Add(column);
            //---
            column = new DataColumn();
            column.DataType = Type.GetType("System.Double");
            column.ColumnName = "Transmittance";
            dTable.Columns.Add(column);
            //---
            column = new DataColumn();
            column.DataType = Type.GetType("System.Double");
            column.ColumnName = "PhaseSpectrum";
            dTable.Columns.Add(column);
            //--- 
            column = new DataColumn();
            column.DataType = Type.GetType("System.Double");
            column.ColumnName = "Envelope+";
            dTable.Columns.Add(column);
            //--
            column = new DataColumn();
            column.DataType = Type.GetType("System.Double");
            column.ColumnName = "Envelope-";
            dTable.Columns.Add(column);
            //---
            Transmittance transmitt = new Transmittance();
            double[,] Results = transmitt.GetTransmittance(filter);
            int size = Results.GetLength(0);
            for (int i = 0; i < size; i++)
            {
                row = dTable.NewRow();
                row["Frequency"] = Results[i, 0];
                row["Transmittance"] = Results[i, 1];
                row["PhaseSpectrum"] = Results[i, 2];
                row["Envelope+"] = Results[i, 3];
                row["Envelope-"] = Results[i, 4];
                dTable.Rows.Add(row);
            }
            //---
            dView = new DataView(dTable);
            //---
            Chart chart01 = new Chart();
            //---
            chart01.Width = 1200;
            chart01.Height = 1000;
            chart01.ChartAreas.Add(new ChartArea("Magnitude"));
            chart01.ChartAreas.Add(new ChartArea("Phase"));
            chart01.ChartAreas.Add(new ChartArea("Current_Magnitude"));

            chart01.ChartAreas["Magnitude"].AxisY.Title = "Amplitude spectrum";
            chart01.ChartAreas["Phase"].AxisY.Title = "Phase spectrum";
            chart01.ChartAreas["Current_Magnitude"].AxisY.Title = "Current Magnitude";

            chart01.DataBindTable(dView, "Frequency");
            //---
            chart01.Series["Transmittance"].ChartType = SeriesChartType.Line;
            chart01.Series["PhaseSpectrum"].ChartType = SeriesChartType.Line;
            chart01.Series["Envelope+"].ChartType = SeriesChartType.Line;
            chart01.Series["Envelope-"].ChartType = SeriesChartType.Line;
            //---
            chart01.Series["Transmittance"].ChartArea = "Magnitude";
            chart01.Series["PhaseSpectrum"].ChartArea = "Phase";
            chart01.Series["Envelope+"].ChartArea = "Current_Magnitude";
            chart01.Series["Envelope-"].ChartArea = "Current_Magnitude";
            //---
            chart01.Titles.Add("Filter Transmittance: U2/U1");

            chart01.ChartAreas[0].AxisX.Title = "Frequency [Hz]";
            chart01.ChartAreas[0].AxisX.LabelStyle.Format = "{#0.0}";
            chart01.ChartAreas[0].AxisX.Minimum = 0;

            chart01.ChartAreas[1].AxisX.Title = "Frequency [Hz]";
            chart01.ChartAreas[1].AxisX.LabelStyle.Format = "{#0.0}";
            chart01.ChartAreas[1].AxisX.Minimum = 0;

            chart01.ChartAreas[2].AxisX.Title = "Frequency [Hz]";
            chart01.ChartAreas[2].AxisX.LabelStyle.Format = "{#0.0}";
            chart01.ChartAreas[2].AxisX.Minimum = 0;
            //---
            chart01.Titles[0].Font =new System.Drawing.Font("Times New Roman", 16F,System.Drawing.FontStyle.Bold);
            //--
            chart01.ChartAreas[0].BackColor = System.Drawing.Color.Azure;
            chart01.ChartAreas[1].BackColor = System.Drawing.Color.Azure;
            chart01.ChartAreas[2].BackColor = System.Drawing.Color.Azure;
            //--
            chart01.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Arial", 12F,System.Drawing.FontStyle.Italic);
            chart01.ChartAreas[1].AxisX.TitleFont = new System.Drawing.Font("Arial", 12F,System.Drawing.FontStyle.Italic);
            chart01.ChartAreas[2].AxisX.TitleFont = new System.Drawing.Font("Arial", 12F,System.Drawing.FontStyle.Italic);
            //--
            chart01.ChartAreas[0].AxisY.TitleFont =new System.Drawing.Font("Times New Roman", 12F,System.Drawing.FontStyle.Bold);
            chart01.ChartAreas[1].AxisY.TitleFont =new System.Drawing.Font("Times New Roman", 12F,System.Drawing.FontStyle.Bold);
            chart01.ChartAreas[2].AxisY.TitleFont =new System.Drawing.Font("Times New Roman", 12F,System.Drawing.FontStyle.Bold);
            //--
            chart01.Series["Transmittance"].Color = System.Drawing.Color.Red;
            chart01.Series["PhaseSpectrum"].Color = System.Drawing.Color.Blue;
            chart01.Series["Envelope+"].Color = System.Drawing.Color.MediumPurple;
            chart01.Series["Envelope-"].Color = System.Drawing.Color.OrangeRed;
            //---
            MemoryStream ms = new MemoryStream();
            chart01.SaveImage(ms, ChartImageFormat.Png);
            return File(ms.GetBuffer(), "image/png");
            }
        }
}
