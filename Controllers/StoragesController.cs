﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ERPNet.Data;
using EmployeeWebMySQL.Models;
using Microsoft.AspNetCore.Cors;

namespace ERPNet.Controllers
{
    [EnableCors ( "AllowSpecificOrigin" )]
    [Route("api/[controller]")]
    [ApiController]
    public class StoragesController : ControllerBase
    {
        private readonly ERPNetContext _context;

        public StoragesController(ERPNetContext context)
        {
            _context = context;
        }

        // GET: api/Storages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Storage>>> GetStorage()
        {
            return await _context.Storage.ToListAsync();
        }

        // GET: api/Storages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Storage>> GetStorage(int id)
        {
            var storage = await _context.Storage.FindAsync(id);

            if (storage == null)
            {
                return NotFound();
            }

            return storage;
        }

        // PUT: api/Storages/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStorage(int id, Storage storage)
        {
            if (id != storage.StorageId)
            {
                return BadRequest();
            }

            _context.Entry(storage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StorageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Storages
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Storage>> PostStorage(Storage storage)
        {
            _context.Storage.Add(storage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStorage", new { id = storage.StorageId }, storage);
        }

        // DELETE: api/Storages/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Storage>> DeleteStorage(int id)
        {
            var storage = await _context.Storage.FindAsync(id);
            if (storage == null)
            {
                return NotFound();
            }

            _context.Storage.Remove(storage);
            await _context.SaveChangesAsync();

            return storage;
        }

        private bool StorageExists(int id)
        {
            return _context.Storage.Any(e => e.StorageId == id);
        }
    }
}
