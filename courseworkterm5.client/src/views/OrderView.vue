<script setup lang="ts">
import { ref } from 'vue';
import SiteButton from '@/components/SiteButton.vue';
import { ButtonSize, ButtonType } from '@/enums/button_enums';
import { useShoppingCartStore, resetShoppingCartState } from '@/stores/shoppingCartStore';
import { useRouter } from 'vue-router';
const shoppingCartStore = useShoppingCartStore();

const emailForSending = ref('duckalan2@gmail.com');

const router = useRouter();

function createOrder() {
    console.log('createorder')
    fetch('/api/sendemail', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json;charset=utf-8'
        },
        body: JSON.stringify({
            userEmail: emailForSending.value,
            products: shoppingCartStore.items.map(i => i.cartItem.name)
        }) 
    })
    .then(response => console.log(response));

    resetShoppingCartState();
    router.push('/');
}
</script>

<template>
    <div class="container" style="margin-top: 1rem;">
        <form @submit.prevent="createOrder()">
            <div>
                <label>
                    E-mail
                    <input type="email" v-model="emailForSending" placeholder="Ваш email" required>
                </label>
            </div>
            <SiteButton
                        :type="ButtonType.Primary"
                        :size="ButtonSize.Medium">
                Оплатить заказ ({{ shoppingCartStore.totalPriceRub }} &#x20bd;)
            </SiteButton>
        </form>

    </div>
</template>