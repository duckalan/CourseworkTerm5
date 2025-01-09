import { defineStore } from 'pinia'
import { ref } from 'vue'

export interface Product {
  productId: number,
  name: string,
  description?: string,
  basePriceRub: number,
  imageUrl: URL
}

export interface ProductCategory {
  productCategoryId: number,
  name: string,
  products: Product[]
}

export const useProductCategoriesStore = defineStore('productCategories', () => {
  const productCategories = ref<ProductCategory[]>([]);
  const isLoading = ref(false);
  const isLoaded = ref(false);

  async function fetchProductCategories() {
    isLoading.value = true;
    const response = await fetch('api/product-categories')

    if (response.ok) {
      productCategories.value = await response.json();
      isLoaded.value = true;
    }
    isLoading.value = false;
  }

  function findProductPriceById(productId: number, productCategoryId?: number): number | null {
    if (productCategoryId) {
      return productCategories.value
        .find(pc => pc.productCategoryId === productCategoryId)
        ?.products.find(p => p.productId === productId)
        ?.basePriceRub
        ?? null
    }

    return productCategories.value
      .flatMap(pc => pc.products)
      .find(p => p.productId === productId)
      ?.basePriceRub
      ?? null
  }

  return { productCategories, isLoading, isLoaded, fetchProductCategories, findProductPriceById }
})
