<script setup lang="ts">
import { onBeforeMount, onMounted } from 'vue';
import { useProductCategoriesStore } from '@/stores/productCategoriesStore';
import NavLink from './NavLink.vue';
import ShoppingCartButton from './ShoppingCartButton.vue';

const productCategoriesStore = useProductCategoriesStore();

onBeforeMount(async () => {
    if (!productCategoriesStore.isLoaded && !productCategoriesStore.isLoading) {
        await productCategoriesStore.fetchProductCategories()
    }
});

onMounted(() => {
    const headerHeight: number = document
        .querySelector('.header')
        ?.clientHeight
        ?? 0;

    const mainNav = document
        .querySelector('.main-nav');

    // What about performance?
    window.addEventListener('scroll', (event) => {
        if (window.scrollY > headerHeight) {
            mainNav?.classList.add('js-sticky-top');
        } else {
            mainNav?.classList.remove('js-sticky-top');
        }
    });
})



</script>

<template>
    <header class="header">
        <div class="container header__container">
            <div>
                Рязань
            </div>
            <div>
                +7 (800) 555-35-35
            </div>
            <div>
                09:00 — 22:00
            </div>
            <div class="login header__login">
                <span class="login__icon"></span>
                <span class="login__text">Войти</span>
            </div>
        </div>
    </header>
    <div class="main-nav">
        <div class="container main-nav__container">
            <div class="brand-logo">
                <NavLink class="brand-logo__link" to="/">
                    <img class="brand-logo__image" :src="'/images/brand_logo.svg'">
                </NavLink>
            </div>

            <nav class="horizontal-nav main-nav__menu">
                <ul class="horizontal-nav__list">
                    <li class="horizontal-nav__item"
                        v-for="productCategory of productCategoriesStore.productCategories"
                        :key="productCategory.productCategoryId">
                        <NavLink :to="{
                            name: 'home',
                            hash: `#${productCategory.name}`
                        }">
                            {{ productCategory.name }}
                        </NavLink>
                    </li>
                </ul>
            </nav>
            <div class="main-nav__shopping-cart">
                <ShoppingCartButton />
            </div>
        </div>
    </div>
</template>


<style lang="less" scoped>
.header {
    background-color: var(--brand-tertiary);
    z-index: 10;

    &__container {
        padding-block: 0.5rem;
        display: flex;
        justify-content: start;
        align-items: center;
        gap: 2rem;
    }

    &__login {
        margin-left: auto;
    }
}

.main-nav {
    background-color: var(--color-background);
    padding-block: 0.5rem;
    z-index: 10;

    &__container {
        display: flex;
        justify-content: start;
        align-items: center;
        flex-wrap: wrap;
        gap: 0.5rem;
    }

    &__shopping-cart {
        margin-left: auto;
    }
}

.brand-logo {
    display: flex;
    align-items: center;

    &__link {
        padding: 0;
        line-height: 1;
    }

    &__image {
        height: 2.5rem;
    }
}

.horizontal-nav {
    &__list {
        list-style: none;
        display: flex;
        justify-content: start;
        align-items: center;
        gap: 0.25rem;
    }
}

.js-sticky-top {
    position: sticky;
    top: 0;
}
</style>