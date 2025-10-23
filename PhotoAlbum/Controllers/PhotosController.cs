using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhotoAlbum.Data;
using PhotoAlbum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PhotoAlbum.Controllers
{

    [Authorize]
    public class PhotosController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly PhotoAlbumContext _context;

        private readonly BlobContainerClient _containerClient;

        // Constructor
        public PhotosController(IConfiguration configuration, PhotoAlbumContext context)
        {
            _context = context;
            _configuration = configuration;

            // Setup blob container client
            var connectionString = _configuration["AzureStorage"];
            var containerName = "photo-album-uploads";
            _containerClient = new BlobContainerClient(connectionString, containerName);
        }

        // GET: Photos/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "Title");

            return View();
        }

        // POST: Photos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PhotoId,Title,Description,Camera,CategoryId,FormFile")] Photo photo)
        {
            // Initialize values
            photo.CreateDate = DateTime.Now;

            // Validate user input
            if (ModelState.IsValid)
            {
                //
                // Step 1: save the file (optionally)
                //
                if(photo.FormFile != null)
                {
                    //
                    // Upload file to Azure Blob Storage
                    //

                    // store the file to upload in fileUpload
                    IFormFile fileUpload = photo.FormFile;

                    // craete a unique filename for the blob
                    string blobName = Guid.NewGuid().ToString() + "_" + fileUpload.FileName;

                    var blobClient = _containerClient.GetBlobClient(blobName);

                    using (var stream = fileUpload.OpenReadStream())
                    {
                        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = fileUpload.ContentType });
                    }

                    string blobURL = blobClient.Uri.ToString();

                    // assign the blob URL to the record to save in Db
                    photo.Filename = blobURL;
                }

                //
                // Step 2: save record in database
                //

                _context.Add(photo);

                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index), "Home"); // Go back to Home Index
            }

            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "Title", photo.CategoryId);
            
            return View(photo);
        }

        // GET: Photos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photo.FindAsync(id);

            if (photo == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "Title", photo.CategoryId);

            return View(photo);
        }

        // POST: Photos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PhotoId,Title,Description,Filename,CreateDate,CategoryId,FormFile")] Photo photo)
        {
            if (id != photo.PhotoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //
                // step 1: save the file
                //
                if(photo.FormFile != null) {

                    // determine new filename

                    // set the new filename in the db record

                    // upload the new file

                    // delete the old file
                }

                //
                // step 2: save in database
                //

                try
                {
                    _context.Update(photo);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotoExists(photo.PhotoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index), "Home"); // Go back to Home Index
            }

            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "CategoryId", "Title", photo.CategoryId);

            return View(photo);
        }

        // GET: Photos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photo = await _context.Photo
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.PhotoId == id);

            if (photo == null)
            {
                return NotFound();
            }

            return View(photo);
        }

        // POST: Photos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photo = await _context.Photo.FindAsync(id);

            if (photo != null)
            {
                _context.Photo.Remove(photo);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), "Home"); // Go back to Home Index
        }

        private bool PhotoExists(int id)
        {
            return _context.Photo.Any(e => e.PhotoId == id);
        }
    }
}
