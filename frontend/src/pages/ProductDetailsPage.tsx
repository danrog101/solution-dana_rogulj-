import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { getProductById } from "../api/products";
import type { Product } from "../types/product";

function ProductDetailsPage() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [product, setProduct] = useState<Product | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    if (id === undefined) {
      return;
    }

    setLoading(true);
    setError("");

    getProductById(id)
      .then((data) => {
        setProduct(data);
        setLoading(false);
      })
      .catch(() => {
        setError("Proizvod nije pronađen.");
        setLoading(false);
      });
  }, [id]);

  if (loading) {
    return <p className="info">Učitavanje...</p>;
  }

  if (error !== "" || product === null) {
    return (
      <div className="info">
        <p className="error">{error}</p>
        <button onClick={() => navigate(-1)}>Natrag na listu</button>
      </div>
    );
  }

  return (
    <div className="container">
      <button onClick={() => navigate(-1)}>Natrag na listu</button>

      <div className="details">
        <div className="gallery">
          {product.images.map((image) => (
            <img src={image} alt={product.title} key={image} />
          ))}
        </div>

        <div className="details-info">
          <h1>{product.title}</h1>
          <p className="category">{product.category}</p>
          <p className="price">{product.price} €</p>
          <p>{product.description}</p>
          <p>Ocjena: {product.rating} / 5</p>
          <p>Na zalihi: {product.stock} kom</p>
          {product.brand !== "" && <p>Brend: {product.brand}</p>}
        </div>
      </div>
    </div>
  );
}

export default ProductDetailsPage;