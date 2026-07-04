import { describe, it, expect, vi, beforeEach } from "vitest";
import { render, screen } from "@testing-library/react";
import { MemoryRouter } from "react-router-dom";
import ProductListPage from "./ProductListPage";
import * as api from "../api/products";

vi.mock("../api/products");

const mockedGetProducts = vi.mocked(api.getProducts);
const mockedGetCategories = vi.mocked(api.getCategories);

function renderPage() {
  render(
    <MemoryRouter>
      <ProductListPage />
    </MemoryRouter>
  );
}

describe("ProductListPage", () => {
  beforeEach(() => {
    mockedGetCategories.mockResolvedValue([]);
  });

  it("prikazuje proizvode nakon ucitavanja", async () => {
    mockedGetProducts.mockResolvedValue({
      items: [
        {
          id: 1,
          title: "Testni proizvod",
          shortDescription: "Kratki opis",
          price: 99.99,
          thumbnail: "slika.jpg",
        },
      ],
      total: 1,
      page: 1,
      pageSize: 12,
    });

    renderPage();

    expect(await screen.findByText("Testni proizvod")).toBeInTheDocument();
    expect(screen.getByText("99.99 €")).toBeInTheDocument();
  });

  it("prikazuje poruku kad nema rezultata", async () => {
    mockedGetProducts.mockResolvedValue({
      items: [],
      total: 0,
      page: 1,
      pageSize: 12,
    });

    renderPage();

    expect(
      await screen.findByText("Nema rezultata za zadane filtere.")
    ).toBeInTheDocument();
  });

  it("prikazuje gresku kad dohvat ne uspije", async () => {
    mockedGetProducts.mockRejectedValue(new Error("greska"));

    renderPage();

    expect(
      await screen.findByText("Nije moguće dohvatiti proizvode. Provjeri radi li backend.")
    ).toBeInTheDocument();
  });
});