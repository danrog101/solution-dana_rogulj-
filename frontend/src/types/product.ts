export interface ProductListItem {
  id: number;
  title: string;
  shortDescription: string;
  price: number;
  thumbnail: string;
}

export interface ProductListResponse {
  items: ProductListItem[];
  total: number;
  page: number;
  pageSize: number;
}

export interface Product {
  id: number;
  title: string;
  description: string;
  category: string;
  price: number;
  thumbnail: string;
  images: string[];
  rating: number;
  stock: number;
  brand: string;
}