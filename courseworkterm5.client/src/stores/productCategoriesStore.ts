import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { Ref } from 'vue';

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
  const productCategories: Ref<ProductCategory[]> = ref([]);
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

  return {productCategories, isLoading, isLoaded, fetchProductCategories }
})
