# Photon-Pun-Lobby-Room-Test
Mini game em que foi utilizado a biblioteca do Photon Pun para criar um Quick Matching Game de Catch Coins com 2 players.

## Execução
Para executar basta baixar o projeto e executar o arquivo localizado em ./Versions/Test Room.exe

![image](https://github.com/tibasoliver/Photon-Pun-Lobby-Room-Test/assets/33914941/b974a40c-a08a-42ac-8582-2b9cd0df32e0)

Ao abrir será apresentado o menu principal aonde o player poderá conectar a rede da photon e consequentemente entrar em uma room preexistente ou criar uma caso não exista pressionando o botão GO.

![image](https://github.com/tibasoliver/Photon-Pun-Lobby-Room-Test/assets/33914941/df0321e1-28a1-49fc-8d0d-31aa527672c7)

Antes de pressionar o botão GO, você pode inserir um nome para o seu player para que seja identificado como tal se assim desejado.
Caso decida não escolher será gerado um nome genérico randomizado para ti.

Obs:. Se deseja testar em sua máquina o processo descrito abaixo não se esqueça de abrir 2 instâncias para que você possa simular a interação entre dois players.

![image](https://github.com/tibasoliver/Photon-Pun-Lobby-Room-Test/assets/33914941/95458078-efc1-4ef8-a526-bf4e273c15d4)

Após entrar em uma room ele aguardará outro player para assim entrar no 'lobby'(seleção de personagem)

![image](https://github.com/tibasoliver/Photon-Pun-Lobby-Room-Test/assets/33914941/06a1982b-42d2-45c1-b9c5-8dd93c038071)

Entrando no 'lobby' você poderá escolher entre os chars disponíveis; e para acenar ao Master Client(que será por padrão o que criou a sala) que está pronto para jogatina pressione o botão READY [não pronto - amarelo/ pronto - verde], incluindo o próprio Master Client.
O personagem mais a esquerda representa o seu char enquanto o outro representa seu rival. Sempre o o char for alterado pelo jogador será mostrado essa alteração na aplicação do outro jogador.

![image](https://github.com/tibasoliver/Photon-Pun-Lobby-Room-Test/assets/33914941/5f75fe6a-1421-48cd-b4e0-151d966cf9e8)

Logo ambos silanizarem que estão prontos o master client encaminharam os jogadores para cena do jogo de fato, aonde é um Catch points, aonde cada maçã vale 10 pontos e nascem de forma aleatoria pelo campo, sendo que a cada 5 segundos aparece 2 novas maçãs.
E no final do tempo cronometrado em que o master client também é o mantenedor será dito quem foi o vencedor ou perdedor ou declaração de empate no jogo.
Obs.: O jogo tem tempo de 20 segundos para facilitar testes, mas o tempo recomendado são 2 minutos. Para alterar o mesmo necessita alterar no código fonte e geral novo build.

![image](https://github.com/tibasoliver/Photon-Pun-Lobby-Room-Test/assets/33914941/86c6e29c-4877-4b76-8ae4-c4d9bdcd1b30)

Ao final do jogo haverá um report com o resultado do game e um botão que lhe conduzirá novamente para o menu principal aonde poderá Criar/Entrar em outro jogo novamente.

![image](https://github.com/tibasoliver/Photon-Pun-Lobby-Room-Test/assets/33914941/ff919263-0cc3-4180-988b-c1f155caf788)

## Features Adicionais

### Permanência no Lobby
Caso o segundo player saia da room você permanecerá na mesma, até que um novo jogador entre na room.

![image](https://github.com/tibasoliver/Photon-Pun-Lobby-Room-Test/assets/33914941/b3d708d2-e71d-4a6f-8f90-30e0e21ab4e9)

### Mostrar velocidade da conexão com o servidor da photon

Feedback da conexão naquele instante.

![image](https://github.com/tibasoliver/Photon-Pun-Lobby-Room-Test/assets/33914941/7f73ed22-85f1-4d28-8b52-7d68bc392490)







