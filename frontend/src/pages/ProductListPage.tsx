import { useEffect, useState } from "react";
import { Link, useSearchParams } from "react-router-dom";
import { getProducts, getCategories } from "../api/products";
import type { ProductListItem } from "../types/product";

function ProductListPage() {
  const [searchParams, setSearchParams] = useSearchParams();

  const search = searchParams.get("search") ?? "";
  const category = searchParams.get("category") ?? "";
  const minPrice = searchParams.get("minPrice") ?? "";
  const maxPrice = searchParams.get("maxPrice") ?? "";
  const page = Number(searchParams.get("page") ?? "1");

  const [searchInput, setSearchInput] = useState(search);
  const [products, setProducts] = useState<ProductListItem[]>([]);
  const [categories, setCategories] = useState<string[]>([]);
  const [total, setTotal] = useState(0);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const pageSize = 12;
  const totalPages = Math.ceil(total / pageSize);

  function updateParam(name: string, value: string) {
    const newParams = new URLSearchParams(searchParams);

    if (value === "") {
      newParams.delete(name);
    } else {
      newParams.set(name, value);
    }

    if (name !== "page") {
      newParams.delete("page");
    }

    setSearchParams(newParams);
  }

  useEffect(() => {
    getCategories()
      .then((data) => {
        setCategories(data);
      })
      .catch(() => {
        setCategories([]);
      });
  }, []);

  useEffect(() => {
    const timer = setTimeout(() => {
      if (searchInput !== search) {
        updateParam("search", searchInput);
      }
    }, 400);

    return () => clearTimeout(timer);
  }, [searchInput]);

  useEffect(() => {
    setLoading(true);
    setError("");

    getProducts({ search, category, minPrice, maxPrice, page })
      .then((data) => {
        setProducts(data.items);
        setTotal(data.total);
        setLoading(false);
      })
      .catch(() => {
        setError("Nije moguće dohvatiti proizvode. Provjeri radi li backend.");
        setLoading(false);
      });
  }, [search, category, minPrice, maxPrice, page]);

  return (
    <div className="container">
      <h1>Katalog proizvoda</h1>

      <div className="filters">
        <input
          type="text"
          placeholder="Pretraži po nazivu..."
          value={searchInput}
          onChange={(e) => setSearchInput(e.target.value)}
        />

        <select value={category} onChange={(e) => updateParam("category", e.target.value)}>
          <option value="">Sve kategorije</option>
          {categories.map((c) => (
            <option value={c} key={c}>
              {c}
            </option>
          ))}
        </select>

        <input
          type="number"
          placeholder="Min cijena"
          value={minPrice}
          onChange={(e) => updateParam("minPrice", e.target.value)}
        />

        <input
          type="number"
          placeholder="Max cijena"
          value={maxPrice}
          onChange={(e) => updateParam("maxPrice", e.target.value)}
        />
      </div>

      {loading && <p className="info">Učitavanje...</p>}

      {!loading && error !== "" && <p className="info error">{error}</p>}

      {!loading && error === "" && products.length === 0 && (
        <p className="info">Nema rezultata za zadane filtere.</p>
      )}

      {!loading && error === "" && products.length > 0 && (
        <div>
          <div className="grid">
            {products.map((product) => (
              <Link to={"/products/" + product.id} key={product.id} className="card">
                <img src={product.thumbnail} alt={product.title} />
                <h2>{product.title}</h2>
                <p>{product.shortDescription}</p>
                <span className="price">{product.price} €</span>
              </Link>
            ))}
          </div>

          <div className="pagination">
            <button
              onClick={() => updateParam("page", String(page - 1))}
              disabled={page <= 1}
            >
              Prethodna
            </button>
            <span>
              Stranica {page} od {totalPages}
            </span>
            <button
              onClick={() => updateParam("page", String(page + 1))}
              disabled={page >= totalPages}
            >
              Sljedeća
            </button>
          </div>
        </div>
      )}
    </div>
  );
}

export default ProductListPage;