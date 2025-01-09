<script setup lang="ts">
import { onBeforeMount } from 'vue';
import ProductCard from '../components/ProductCard.vue'
import { useProductCategoriesStore } from '@/stores/productCategoriesStore'

const productCategoriesStore = useProductCategoriesStore();

onBeforeMount(async () => {
  if (!productCategoriesStore.isLoaded && !productCategoriesStore.isLoading) {
    await productCategoriesStore.fetchProductCategories()
  }
})
</script>

<template>
  <main>
    <div class="container">
      <section v-for="productCategory of productCategoriesStore.productCategories"
               class="product-category"
               :key="productCategory.productCategoryId"
               :id="`${productCategory.name}`">

        <h1 class="product-category__title">
          {{ productCategory.name }}
        </h1>

        <div class="product-category__cards">
          <ProductCard v-for="product of productCategory.products"
                       :product="product"
                       :is-pizza="productCategory.name === 'Пиццы'" />
        </div>

      </section>
    </div>
  </main>
</template>

<style lang="less" scoped>
.product-category {
  &__title {
    font-size: var(--text-xxl);
    padding: 0.5rem;
    margin-bottom: 0.5rem;
  }

  &__cards {
    display: grid;
    grid-template-columns: repeat(4, 1fr);
    gap: 1.5rem;
    margin-bottom: 2rem;

    @media screen and (max-width: 1024px) {
      grid-template-columns: repeat(3, 1fr);
    }

    @media screen and (max-width: 767px) {
      grid-template-columns: repeat(2, 1fr);
    }

    @media screen and (max-width: 426px) {
      grid-template-columns: repeat(1, 1fr);
    }
  }
}
</style>
