<script setup lang="ts">
import { ButtonType, ButtonSize } from '@/enums/button_enums'
import SiteButton from '@/components/SiteButton.vue';
import type { Product } from '@/stores/productCategoriesStore';

defineProps<{
    product: Product,
    isPizza: boolean
}>()
</script>

<template>
    <article class="card">
        <header class="card__header">
            <img class="card__image"
                 :src="product.imageUrl.toString()"
                 :alt="`Картинка продукта ${product.name}`"
                 loading="lazy">
        </header>

        <main class="card__main">
            <h2 class="card__name">{{ product.name }}</h2>
            <p class="card__ingridients">{{ product.description }}</p>
        </main>

        <footer class="card__footer">
            
            <template v-if="isPizza">
                <span class="card__base-price">
                    от <span class="card__price">
                        {{ product.basePriceRub }}
                    </span> &#x20bd;
                </span>

                <SiteButton :type="ButtonType.Primary"
                            :size="ButtonSize.Medium"
                            onclick="alert('Hello')">
                    Выбрать
                </SiteButton>
            </template>

            <template v-else>
                <span class="card__base-price">
                    <span class="card__price">
                        {{ product.basePriceRub }}
                    </span> &#x20bd;
                </span>

                <SiteButton :type="ButtonType.Primary"
                            :size="ButtonSize.Medium"
                            onclick="alert('В корзину')">
                    В корзину
                </SiteButton>
            </template>

        </footer>
    </article>

</template>

<style scoped lang="less">
.card {
    color: var(--color-text);
    background: var(--color-background);
    border-radius: var(--border-radius-container);
    padding: 1rem;
    display: flex;
    flex-direction: column;
    justify-content: center;

    &__image {
        width: 100%;
        height: auto;
    }

    &__main {
        flex: 1;
        margin-bottom: 0.75rem;
    }

    &__name {
        font-size: var(--text-lg);
        margin-bottom: 0.5rem;
    }

    &__ingridients {
        font-size: var(--text-sm);
        color: var(--color-text-muted);
        line-height: 1.3;
    }

    &__footer {
        display: flex;
        justify-content: space-between;
        align-items: center;
        gap: 1rem;
        flex-wrap: wrap-reverse;
    }

    &__price {
        font-weight: 500;
    }
}
</style>
