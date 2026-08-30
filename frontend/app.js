// ============================================================
//  MyShop — Session 19  ·  Full-stack front end
//  This page is the CLIENT. All data lives in your C# Web API
//  (../backend) which stores it in a real SQLite database.
//  The page never keeps its own list — it always asks the API.
//
//  ONE setting: where your API is running. When you start the
//  backend with `dotnet run`, it prints this address.
const API_BASE = "http://localhost:5000/api/products";
// ============================================================

const grid     = document.querySelector("#product-grid");
const statusEl = document.querySelector("#status");
const form     = document.querySelector("#add-form");

// ---------- Talking to the API ----------
// GET  api/products        -> load every product
async function apiGetAll() {
  const res = await fetch(API_BASE);
  if (!res.ok) throw new Error("GET failed: " + res.status);
  return await res.json();
}

// POST api/products        -> create one product
async function apiCreate(product) {
  const res = await fetch(API_BASE, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(product)
  });
  if (!res.ok) throw new Error("POST failed: " + res.status);
}

// DELETE api/products/{id} -> remove one product
async function apiDelete(id) {
  const res = await fetch(`${API_BASE}/${id}`, { method: "DELETE" });
  if (!res.ok) throw new Error("DELETE failed: " + res.status);
}

// ---------- Rendering ----------
function render(list) {
  statusEl.className = "";
  statusEl.textContent = `Connected to the API — ${list.length} product(s).`;

  if (list.length === 0) {
    grid.innerHTML = "<p>No products yet. Add one below!</p>";
    return;
  }
  grid.innerHTML = list.map(p => `
    <article class="card">
      <h3>${p.name}</h3>
      <p class="desc">${p.description ?? ""}</p>
      <p class="price">$${Number(p.price).toFixed(2)}</p>
      <button class="btn-delete" data-id="${p.id}">Delete</button>
    </article>
  `).join("");
}

function showError() {
  statusEl.className = "status-error";
  statusEl.textContent =
    "Can't reach the API. Start the backend first: open the /backend folder " +
    "in a terminal and run  dotnet run  (it should be at " + API_BASE + ").";
  grid.innerHTML = "";
}

// Load everything from the API and draw it.
async function refresh() {
  try {
    const products = await apiGetAll();
    render(products);
  } catch (err) {
    console.error(err);
    showError();
  }
}

// ---------- Events ----------
// Delete (event delegation — one listener handles every card's button)
grid.addEventListener("click", async (e) => {
  if (!e.target.classList.contains("btn-delete")) return;
  try {
    await apiDelete(e.target.dataset.id);
    refresh();                              // re-load from the API
  } catch (err) {
    console.error(err);
    showError();
  }
});

// Add (from the form)
form.addEventListener("submit", async (e) => {
  e.preventDefault();                       // stop the page from reloading

  const name  = document.querySelector("#name").value.trim();
  const price = document.querySelector("#price").value;
  const description = document.querySelector("#description").value.trim();

  // Simple client-side validation
  if (name === "" || price === "" || Number(price) < 0) {
    alert("Please enter a name and a price of 0 or more.");
    return;
  }

  try {
    await apiCreate({ name, price: Number(price), description });
    form.reset();
    refresh();                              // re-load so the new item appears
  } catch (err) {
    console.error(err);
    showError();
  }
});

// Kick things off — ask the API for the current list.
refresh();
