import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { getProducts } from "../api/products";
import type { ProductListItem } from "../types/product";
function ProductListPage() {
  const [products, setProducts] = useState<ProductListItem[]>([]);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const pageSize = 12;
  const totalPages = Math.ceil(total / pageSize);

  useEffect(() => {
    setLoading(true);
    setError("");

    getProducts(page)
      .then((data) => {
        setProducts(data.items);
        setTotal(data.total);
        setLoading(false);
      })
      .catch(() => {
        setError("Nije moguće dohvatiti proizvode. Provjeri radi li backend.");
        setLoading(false);
      });
  }, [page]);

  if (loading) {
    return <p className="info">Učitavanje...</p>;
  }

  if (error !== "") {
    return <p className="info error">{error}</p>;
  }

  if (products.length === 0) {
    return <p className="info">Nema rezultata.</p>;
  }

  return (
    <div className="container">
      <h1>Katalog proizvoda</h1>

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
        <button onClick={() => setPage(page - 1)} disabled={page === 1}>
          Prethodna
        </button>
        <span>
          Stranica {page} od {totalPages}
        </span>
        <button onClick={() => setPage(page + 1)} disabled={page === totalPages}>
          Sljedeća
        </button>
      </div>
    </div>
  );
}

export default ProductListPage;