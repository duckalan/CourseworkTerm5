<script setup lang="ts">
import { onBeforeMount } from 'vue';
import ProductCard from '../components/ProductCard.vue'
import { useProductCategoriesStore } from '@/stores/productCategoriesStore'

const productCategoriesStore = useProductCategoriesStore();

onBeforeMount(async () => {
  if (!productCategoriesStore.isLoaded) {
    console.log('fetching')
    await productCategoriesStore.fetchProductCategories()
  }
})
</script>

<template>
  <main>
    <div class="container">
      <section v-for="productCategory of productCategoriesStore.productCategories"
               class="product-section"
               :key="productCategory.productCategoryId"
               :id="`${productCategory.name}`">

        <h1 class="product-section__title">
          {{ productCategory.name }}
        </h1>

        <div class="product-section__cards">
          <ProductCard v-for="product of productCategory.products"
                       :product="product"
                       :is-pizza="productCategory.name === 'Пиццы'"
                       />
        </div>

      </section>
    </div>
  </main>
</template>

<style lang="less" scoped>
.product-section {
  &__title {
    font-size: var(--text-xxxl);
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
